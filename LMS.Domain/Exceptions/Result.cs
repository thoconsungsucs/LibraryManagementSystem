namespace LMS.Domain.Exceptions
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public Error Error { get; }
        public Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, Error.None);
        public static Result Failure(Error error) => new Result(false, error);
        public static Result<TValue> Success<TValue>(TValue value) => new Result<TValue>(value, true, Error.None);
        public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(default, false, error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;
        public Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("There is no value for failure result");
        public static implicit operator Result<TValue>(TValue value) => value != null ? Success(value) : Failure<TValue>(Error.Unknown);
        public static Result<TValue> ValidationFailure(Error error) => new Result<TValue>(default, false, error);
    }
}
