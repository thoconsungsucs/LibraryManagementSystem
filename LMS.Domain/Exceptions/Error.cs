using FluentValidation.Results;
using LMS.Domain.Ultilities;

namespace LMS.Domain.Exceptions
{
    public record Error(string code, Dictionary<string, List<string>>? errors = null)
    {
        public static readonly Error None = new(string.Empty);
        public static readonly Error NullObject = new(SD.ErrorCode.NullObject);
    }

    public record ValidationError : Error
    {
        public ValidationError(ValidationResult result)
            : base
            (
                  SD.ErrorCode.ValidationError,
                  result.Errors
                      .GroupBy(e => e.PropertyName)
                      .ToDictionary(
                          g => g.Key,
                          g => g.Select(e => e.ErrorMessage).ToList()
                      )
            )
        {
        }
    }


    public record InternalServerError : Error
    {
        public InternalServerError(String exMessage) 
            : base
            (
                SD.ErrorCode.InternalServerError,
                new Dictionary<string, List<string>> { { "Message", new List<string> { exMessage } } }
            )
        {
        }
    }

    public static class BookError
    {
        public static Error NotFound(int id) => new Error(
            SD.ErrorCode.NotFound,
            new Dictionary<string, List<string>> { { "Id", new List<string> { $"Book with id {id} not found" } } }
        );

        public static Error InsufficientStock() => new Error(
            SD.ErrorCode.InsufficientStock,
            new Dictionary<string, List<string>> { { "Available", new List<string> { SD.ValidationMessage.BookMessage.NotAvailable } } }
        );
    }

    public static class UserError
    {
        public static Error NotFound(int id) => new Error(
            SD.ErrorCode.NotFound,
            new Dictionary<string, List<string>> { { "Id", new List<string> { $"User with id {id} not found" } } }
        );

        public static Error Unauthorized() => new Error(
            SD.ErrorCode.Unauthorized,
            new Dictionary<string, List<string>> { { "Id", new List<string> { $"Unauthorized" } } }
        );

        public static Error InvalidCredentials() => new Error(
            SD.ErrorCode.Unauthorized,
            new Dictionary<string, List<string>>
            {
                { "Username", new List<string> { $"Not found or invalid" } },
                { "Password", new List<string> { $"Not found or invalid" } }
            }
        );
    }
}
