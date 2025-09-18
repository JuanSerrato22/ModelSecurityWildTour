using Business.Interfaces;
using Entity.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Results;

namespace Business.Decorators
{
    public class LoggingActivityServiceDecorator : IActivityService
    {
        private readonly IActivityService _activityService;
        private readonly ILogger<LoggingActivityServiceDecorator> _logger;

        public LoggingActivityServiceDecorator(
            IActivityService activityService,
            ILogger<LoggingActivityServiceDecorator> logger)
        {
            _activityService = activityService ?? throw new ArgumentNullException(nameof(activityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<IEnumerable<ActivityDTO>>> GetAllAsync()
        {
            _logger.LogInformation("Iniciando obtención de todas las actividades");
            var result = await _activityService.GetAllAsync();

            if (result.IsSuccess)
                _logger.LogInformation("Se obtuvieron {Count} actividades exitosamente", ((IEnumerable<ActivityDTO>)result.Data).Count());
            else
                _logger.LogWarning("Falló la obtención de actividades: {Message}", result.Message);

            return result;
        }

        public async Task<ServiceResult<ActivityDTO>> GetByIdAsync(int id)
        {
            _logger.LogInformation("Iniciando obtención de actividad con ID: {ActivityId}", id);
            var result = await _activityService.GetByIdAsync(id);

            if (result.IsSuccess)
                _logger.LogInformation("Actividad con ID {ActivityId} obtenida exitosamente", id);
            else
                _logger.LogWarning("Falló la obtención de actividad con ID {ActivityId}: {Message}", id, result.Message);

            return result;
        }

        public async Task<ServiceResult<ActivityDTO>> CreateAsync(ActivityDTO activityDto)
        {
            _logger.LogInformation("Iniciando creación de actividad: {ActivityName}", activityDto?.Name);
            var result = await _activityService.CreateAsync(activityDto);

            if (result.IsSuccess)
                _logger.LogInformation("Actividad '{ActivityName}' creada exitosamente", activityDto.Name);
            else
                _logger.LogWarning("Falló la creación de actividad '{ActivityName}': {Message}", activityDto?.Name, result.Message);

            return result;
        }

        public async Task<ServiceResult<ActivityDTO>> UpdateAsync(int id, ActivityDTO activityDto)
        {
            _logger.LogInformation("Iniciando actualización de actividad con ID: {ActivityId}", id);
            var result = await _activityService.UpdateAsync(id, activityDto);

            if (result.IsSuccess)
                _logger.LogInformation("Actividad con ID {ActivityId} actualizada exitosamente", id);
            else
                _logger.LogWarning("Falló la actualización de actividad con ID {ActivityId}: {Message}", id, result.Message);

            return result;
        }

        public async Task<ServiceResult<ActivityDTO>> UpdatePartialAsync(int id, JsonPatchDocument<ActivityDTO> patchDoc)
        {
            _logger.LogInformation("Iniciando actualización parcial de actividad con ID: {ActivityId}", id);
            var result = await _activityService.UpdatePartialAsync(id, patchDoc);

            if (result.IsSuccess)
                _logger.LogInformation("Actividad con ID {ActivityId} actualizada parcialmente exitosamente", id);
            else
                _logger.LogWarning("Falló la actualización parcial de actividad con ID {ActivityId}: {Message}", id, result.Message);

            return result;
        }

        public async Task<ServiceResult> SoftDeleteAsync(int id)
        {
            _logger.LogInformation("Iniciando eliminación lógica de actividad con ID: {ActivityId}", id);
            var result = await _activityService.SoftDeleteAsync(id);

            if (result.IsSuccess)
                _logger.LogInformation("Actividad con ID {ActivityId} eliminada/restaurada lógicamente", id);
            else
                _logger.LogWarning("Falló la eliminación lógica de actividad con ID {ActivityId}: {Message}", id, result.Message);

            return result;
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            _logger.LogInformation("Iniciando eliminación permanente de actividad con ID: {ActivityId}", id);
            var result = await _activityService.DeleteAsync(id);

            if (result.IsSuccess)
                _logger.LogInformation("Actividad con ID {ActivityId} eliminada permanentemente", id);
            else
                _logger.LogWarning("Falló la eliminación permanente de actividad con ID {ActivityId}: {Message}", id, result.Message);

            return result;
        }

        public async Task<ServiceResult<IEnumerable<ActivityDTO>>> GetActivitiesByCategoryAsync(string category)
        {
            _logger.LogInformation("Iniciando obtención de actividades por categoría: {Category}", category);
            var result = await _activityService.GetActivitiesByCategoryAsync(category);

            if (result.IsSuccess)
                _logger.LogInformation("Se obtuvieron {Count} actividades para la categoría {Category}", ((IEnumerable<ActivityDTO>)result.Data).Count(), category);
            else
                _logger.LogWarning("Falló la obtención de actividades por categoría {Category}: {Message}", category, result.Message);

            return result;
        }

        public async Task<ServiceResult<IEnumerable<ActivityDTO>>> GetActiveActivitiesAsync()
        {
            _logger.LogInformation("Iniciando obtención de actividades activas");
            var result = await _activityService.GetActiveActivitiesAsync();

            if (result.IsSuccess)
                _logger.LogInformation("Se obtuvieron {Count} actividades activas", ((IEnumerable<ActivityDTO>)result.Data).Count());
            else
                _logger.LogWarning("Falló la obtención de actividades activas: {Message}", result.Message);

            return result;
        }
    }
}