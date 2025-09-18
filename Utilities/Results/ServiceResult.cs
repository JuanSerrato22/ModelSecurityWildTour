using System.Collections.Generic;

namespace Utilities.Results
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string Message { get; private set; }
        public List<string> Errors { get; private set; }

        private ServiceResult(bool isSuccess, T? data, string message, List<string>? errors = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            Errors = errors ?? new List<string>();
        }

        public static ServiceResult<T> Success(T data, string message = "Operación completada exitosamente")
        {
            return new ServiceResult<T>(true, data, message);
        }

        public static ServiceResult<T> Failure(string message, List<string>? errors = null)
        {
            return new ServiceResult<T>(false, default, message, errors);
        }

        public static ServiceResult<T> Failure(string message, string error)
        {
            return new ServiceResult<T>(false, default, message, new List<string> { error });
        }

        public static ServiceResult<T> NotFound(string message = "Recurso no encontrado")
        {
            return new ServiceResult<T>(false, default, message, new List<string> { "RESOURCE_NOT_FOUND" });
        }
    }

    public class ServiceResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public List<string> Errors { get; private set; }

        private ServiceResult(bool isSuccess, string message, List<string>? errors = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = errors ?? new List<string>();
        }

        public static ServiceResult Success(string message = "Operación completada exitosamente")
        {
            return new ServiceResult(true, message);
        }

        public static ServiceResult Failure(string message, List<string>? errors = null)
        {
            return new ServiceResult(false, message, errors);
        }

        public static ServiceResult Failure(string message, string error)
        {
            return new ServiceResult(false, message, new List<string> { error });
        }

        public static ServiceResult NotFound(string message = "Recurso no encontrado")
        {
            return new ServiceResult(false, message, new List<string> { "RESOURCE_NOT_FOUND" });
        }
    }
}