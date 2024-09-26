using LMS.Domain.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Domain.Exceptions
{
    public static class ApiResult
    {
        public static ProblemDetails ToProblemDetails(Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }
            return result.Error.code switch
            {
                SD.ErrorCode.ValidationError => new ProblemDetails
                {
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Extensions = GetError(result.Error)
                },

                SD.ErrorCode.NotFound => new ProblemDetails
                {
                    Title = "The specified resource was not found.",
                    Status = StatusCodes.Status404NotFound,
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.4",
                    Extensions = GetError(result.Error)
                },

                SD.ErrorCode.InsufficientStock => new ProblemDetails
                {
                    Title = "Insufficient Stock",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Extensions = GetError(result.Error)
                },

                SD.ErrorCode.Unauthorized => new ProblemDetails
                {
                    Title = "Unauthorized",
                    Status = StatusCodes.Status401Unauthorized,
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.3",
                    Extensions = GetError(result.Error)
                },

                _ => new ProblemDetails
                {
                    Title = "An error occurred while processing your request.",
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                    Extensions = GetError(result.Error)
                }
            };


            Dictionary<string, object?> GetError(Error error)
            {
                return new Dictionary<string, object?>
                {
                    ["errors"] = result.Error.errors ?? new Dictionary<string, List<string>>()
                };
            }
        }
    }
}

