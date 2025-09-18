using Entity.DTO;
using System.Collections.Generic;
using System.Linq;
using Utilities.Results;

namespace Business.Validators
{
    public static class ActivityValidator
    {
        public static ServiceResult ValidateForCreation(ActivityDTO activityDto)
        {
            var errors = new List<string>();

            if (activityDto == null)
            {
                return ServiceResult.Failure("El objeto Activity no puede ser nulo", "ACTIVITY_NULL");
            }

            if (string.IsNullOrWhiteSpace(activityDto.Name))
            {
                errors.Add("El nombre de la actividad es requerido");
            }
            else if (activityDto.Name.Length > 100)
            {
                errors.Add("El nombre de la actividad no puede exceder 100 caracteres");
            }

            if (string.IsNullOrWhiteSpace(activityDto.Description))
            {
                errors.Add("La descripción de la actividad es requerida");
            }
            else if (activityDto.Description.Length > 500)
            {
                errors.Add("La descripción no puede exceder 500 caracteres");
            }

            if (string.IsNullOrWhiteSpace(activityDto.Category))
            {
                errors.Add("La categoría de la actividad es requerida");
            }

            if (activityDto.Price < 0)
            {
                errors.Add("El precio no puede ser negativo");
            }

            if (activityDto.Price > 999999.99m)
            {
                errors.Add("El precio no puede exceder 999,999.99");
            }

            return errors.Any()
                ? ServiceResult.Failure("Errores de validación encontrados", errors)
                : ServiceResult.Success("Validación exitosa");
        }

        public static ServiceResult ValidateForUpdate(int id, ActivityDTO activityDto)
        {
            if (id <= 0)
            {
                return ServiceResult.Failure("El ID debe ser mayor a 0", "INVALID_ID");
            }

            return ValidateForCreation(activityDto);
        }

        public static ServiceResult ValidateId(int id)
        {
            return id <= 0
                ? ServiceResult.Failure("El ID debe ser mayor a 0", "INVALID_ID")
                : ServiceResult.Success("ID válido");
        }

        public static ServiceResult ValidateCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return ServiceResult.Failure("La categoría no puede estar vacía", "INVALID_CATEGORY");
            }

            if (category.Length > 50)
            {
                return ServiceResult.Failure("La categoría no puede exceder 50 caracteres", "INVALID_CATEGORY");
            }

            return ServiceResult.Success("Categoría válida");
        }
    }
}