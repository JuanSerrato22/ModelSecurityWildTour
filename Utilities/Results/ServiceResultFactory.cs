using System.Collections.Generic;
using System.Linq;

namespace Utilities.Results
{
    public static class ServiceResultFactory
    {
        public static ServiceResult<T> Success<T>(T data, string message = "Operación exitosa")
        {
            return ServiceResult<T>.Success(data, message);
        }

        public static ServiceResult Success(string message = "Operación exitosa")
        {
            return ServiceResult.Success(message);
        }

        public static ServiceResult<T> NotFound<T>(string message)
        {
            return ServiceResult<T>.NotFound(message);
        }

        public static ServiceResult NotFound(string message)
        {
            return ServiceResult.NotFound(message);
        }

        public static ServiceResult<T> ValidationFailure<T>(string message, IEnumerable<string> errors)
        {
            return ServiceResult<T>.Failure(message, errors.ToList());
        }

        public static ServiceResult ValidationFailure(string message, IEnumerable<string> errors)
        {
            return ServiceResult.Failure(message, errors.ToList());
        }

        public static ServiceResult<T> InternalError<T>(string userMessage = "Error interno del servidor", string technicalMessage = "")
        {
            return ServiceResult<T>.Failure(userMessage, technicalMessage);
        }

        public static ServiceResult InternalError(string userMessage = "Error interno del servidor", string technicalMessage = "")
        {
            return ServiceResult.Failure(userMessage, technicalMessage);
        }
    }
}