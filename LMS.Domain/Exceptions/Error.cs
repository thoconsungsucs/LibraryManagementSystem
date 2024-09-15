using FluentValidation.Results;

namespace LMS.Domain.Exceptions
{
    public record Error(string code, string? description = null)
    {
        public static readonly Error None = new(string.Empty);
        public static readonly Error Unknown = new("Unknown", "An unknown error occurred, returned value is null");
    }

    public record ValidationError : Error
    {
        public ValidationError(ValidationResult result)
            : base("ValidationError", string.Join(", ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")))
        {
        }
    }
}
