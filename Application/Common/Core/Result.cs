using System.Collections.Generic;

namespace Application.Common.Core
{
    public class Result<T>
    {
        public bool IsSuccess {get; set;}
        public T Value { get; set; }
        public List<Error> Errors { get; set; }

        public static Result<T> Success(T value) => new Result<T> {IsSuccess = true, Value = value, Errors = new List<Error>() };
        public static Result<T> Failure(Error error) => new Result<T> {IsSuccess = false, Errors =  new List<Error>() {error} };
        public static Result<T> Failure(List<Error> errors) => new Result<T> {IsSuccess = false, Errors = errors };
    }
}