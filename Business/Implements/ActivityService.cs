using AutoMapper;
using Business.Interfaces;
using Business.Specifications;
using Business.Validators;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Results;

namespace Business.Implements
{
    public class ActivityService : IActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActivityService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResult<IEnumerable<ActivityDTO>>> GetAllAsync()
        {
            var activities = await _unitOfWork.ActivityRepository.GetAllAsync();
            var activitiesDto = _mapper.Map<IEnumerable<ActivityDTO>>(activities);
            return ServiceResultFactory.Success(activitiesDto, "Actividades obtenidas exitosamente");
        }

        public async Task<ServiceResult<ActivityDTO>> GetByIdAsync(int id)
        {
            var validationResult = ActivityValidator.ValidateId(id);
            if (!validationResult.IsSuccess)
            {
                return ServiceResultFactory.ValidationFailure<ActivityDTO>(validationResult.Message, validationResult.Errors);
            }

            var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(id);
            if (activity == null)
            {
                return ServiceResultFactory.NotFound<ActivityDTO>($"La actividad con ID {id} no fue encontrada");
            }

            var activityDto = _mapper.Map<ActivityDTO>(activity);
            return ServiceResultFactory.Success(activityDto, "Actividad obtenida exitosamente");
        }

        public async Task<ServiceResult<ActivityDTO>> CreateAsync(ActivityDTO activityDto)
        {
            var validationResult = ActivityValidator.ValidateForCreation(activityDto);
            if (!validationResult.IsSuccess)
            {
                return ServiceResultFactory.ValidationFailure<ActivityDTO>(validationResult.Message, validationResult.Errors);
            }

            var activity = _mapper.Map<Activity>(activityDto);
            activity.CreatedAt = DateTime.UtcNow;
            activity.Active = true;

            var createdActivity = await _unitOfWork.ActivityRepository.CreateAsync(activity);
            await _unitOfWork.SaveChangesAsync();

            var createdActivityDto = _mapper.Map<ActivityDTO>(createdActivity);
            return ServiceResultFactory.Success(createdActivityDto, "Actividad creada exitosamente");
        }

        public async Task<ServiceResult<ActivityDTO>> UpdateAsync(int id, ActivityDTO activityDto)
        {
            var validationResult = ActivityValidator.ValidateForUpdate(id, activityDto);
            if (!validationResult.IsSuccess)
            {
                return ServiceResultFactory.ValidationFailure<ActivityDTO>(validationResult.Message, validationResult.Errors);
            }

            var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(id);
            if (activity == null)
            {
                return ServiceResultFactory.NotFound<ActivityDTO>($"La actividad con ID {id} no fue encontrada");
            }

            activity.Name = activityDto.Name;
            activity.Description = activityDto.Description;
            activity.Category = activityDto.Category;
            activity.Price = activityDto.Price;

            var updatedActivity = await _unitOfWork.ActivityRepository.UpdateAsync(activity);
            await _unitOfWork.SaveChangesAsync();

            var updatedActivityDto = _mapper.Map<ActivityDTO>(updatedActivity);
            return ServiceResultFactory.Success(updatedActivityDto, "Actividad actualizada exitosamente");
        }

        public async Task<ServiceResult<ActivityDTO>> UpdatePartialAsync(int id, JsonPatchDocument<ActivityDTO> patchDoc)
        {
            var idValidationResult = ActivityValidator.ValidateId(id);
            if (!idValidationResult.IsSuccess)
            {
                return ServiceResultFactory.ValidationFailure<ActivityDTO>(idValidationResult.Message, idValidationResult.Errors);
            }

            if (patchDoc == null)
            {
                return ServiceResultFactory.ValidationFailure<ActivityDTO>("El documento de patch no puede ser nulo", new[] { ErrorCodes.PATCH_DOC_NULL });
            }

            var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(id);
            if (activity == null)
            {
                return ServiceResultFactory.NotFound<ActivityDTO>($"La actividad con ID {id} no fue encontrada");
            }

            var activityDto = _mapper.Map<ActivityDTO>(activity);
            patchDoc.ApplyTo(activityDto);

            var validationResult = ActivityValidator.ValidateForCreation(activityDto);
            if (!validationResult.IsSuccess)
            {
                return ServiceResultFactory.ValidationFailure<ActivityDTO>(validationResult.Message, validationResult.Errors);
            }

            _mapper.Map(activityDto, activity);

            var updatedActivity = await _unitOfWork.ActivityRepository.UpdateAsync(activity);
            await _unitOfWork.SaveChangesAsync();

            var updatedActivityDto = _mapper.Map<ActivityDTO>(updatedActivity);
            return ServiceResultFactory.Success(updatedActivityDto, "Actividad actualizada parcialmente exitosamente");
        }

        public async Task<ServiceResult> SoftDeleteAsync(int id)
        {
            var validationResult = ActivityValidator.ValidateId(id);
            if (!validationResult.IsSuccess)
            {
                return ServiceResultFactory.ValidationFailure(validationResult.Message, validationResult.Errors);
            }

            var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(id);
            if (activity == null)
            {
                return ServiceResultFactory.NotFound($"La actividad con ID {id} no fue encontrada");
            }

            if (activity.Active)
            {
                activity.SoftDelete();
            }
            else
            {
                activity.Restore();
            }

            await _unitOfWork.ActivityRepository.UpdateAsync(activity);
            await _unitOfWork.SaveChangesAsync();

            var action = activity.Active ? "restaurada" : "eliminada lógicamente";
            return ServiceResultFactory.Success($"Actividad {action} exitosamente");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var validationResult = ActivityValidator.ValidateId(id);
            if (!validationResult.IsSuccess)
            {
                return ServiceResultFactory.ValidationFailure(validationResult.Message, validationResult.Errors);
            }

            var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(id);
            if (activity == null)
            {
                return ServiceResultFactory.NotFound($"La actividad con ID {id} no fue encontrada");
            }

            var success = await _unitOfWork.ActivityRepository.DeleteAsync(id);
            if (!success)
            {
                return ServiceResultFactory.ValidationFailure("No se pudo eliminar la actividad", new[] { ErrorCodes.DELETE_FAILED });
            }

            await _unitOfWork.SaveChangesAsync();
            return ServiceResultFactory.Success("Actividad eliminada permanentemente exitosamente");
        }

        public async Task<ServiceResult<IEnumerable<ActivityDTO>>> GetActivitiesByCategoryAsync(string category)
        {
            var validationResult = ActivityValidator.ValidateCategory(category);
            if (!validationResult.IsSuccess)
            {
                return ServiceResultFactory.ValidationFailure<IEnumerable<ActivityDTO>>(validationResult.Message, validationResult.Errors);
            }

            var spec = new ActivitiesByCategorySpecification(category).And(new ActiveActivitiesSpecification());
            var activities = await _unitOfWork.ActivityRepository.GetAllAsync();
            var filteredActivities = activities.Where(spec.ToExpression().Compile()).ToList();

            var activitiesDto = _mapper.Map<IEnumerable<ActivityDTO>>(filteredActivities);
            return ServiceResultFactory.Success(activitiesDto, $"Actividades de la categoría '{category}' obtenidas exitosamente");
        }

        public async Task<ServiceResult<IEnumerable<ActivityDTO>>> GetActiveActivitiesAsync()
        {
            var spec = new ActiveActivitiesSpecification();
            var activities = await _unitOfWork.ActivityRepository.GetAllAsync();
            var activeActivities = activities.Where(spec.ToExpression().Compile()).ToList();

            var activitiesDto = _mapper.Map<IEnumerable<ActivityDTO>>(activeActivities);
            return ServiceResultFactory.Success(activitiesDto, "Actividades activas obtenidas exitosamente");
        }
    }
}