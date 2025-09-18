using System.Collections.Generic;

namespace Utilities.Results
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string Message { get; private set; }
        public List<string> Errors { get; private set; }

        private Result(bool isSuccess, T? data, string message, List<string>? errors = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            Errors = errors ?? new List<string>();
        }

        public static Result<T> Success(T data, string message = "Operation completed successfully")
        {
            return new Result<T>(true, data, message);
        }

        public static Result<T> Failure(string message, List<string>? errors = null)
        {
            return new Result<T>(false, default, message, errors);
        }

        public static Result<T> Failure(string message, string error)
        {
            return new Result<T>(false, default, message, new List<string> { error });
        }
    }

    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public List<string> Errors { get; private set; }

        private Result(bool isSuccess, string message, List<string>? errors = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = errors ?? new List<string>();
        }

        public static Result Success(string message = "Operation completed successfully")
        {
            return new Result(true, message);
        }

        public static Result Failure(string message, List<string>? errors = null)
        {
            return new Result(false, message, errors);
        }

        public static Result Failure(string message, string error)
        {
            return new Result(false, message, new List<string> { error });
        }
    }
}