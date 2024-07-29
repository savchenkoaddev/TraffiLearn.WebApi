using TraffiLearn.Domain.Shared;

namespace TraffiLearn.WebAPI.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            var error = result.Error;

            if (result is IValidationResult validationResult)
            {
                return Results.Problem(
                    statusCode: GetStatusCode(error.ErrorType),
                    title: GetTitle(error.ErrorType),
                    type: GetType(error.ErrorType),
                    extensions: new Dictionary<string, object?>
                    {
                        { "errors", new object[] { validationResult.Errors } }
                    });
            }

            return Results.Problem(
                statusCode: GetStatusCode(error.ErrorType),
                title: GetTitle(error.ErrorType),
                type: GetType(error.ErrorType),
                extensions: new Dictionary<string, object?>
                    {
                        { "errors", new object[] { result.Error } }
                    });
        }

        private static int GetStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.OperationFailure => StatusCodes.Status400BadRequest,

                _ => StatusCodes.Status500InternalServerError
            };
        }

        private static string GetTitle(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.NotFound => "Not Found",
                ErrorType.OperationFailure => "Bad Request",

                _ => "Internal Server Error"
            };
        }

        private static string GetType(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorType.OperationFailure => "https://tools.ietf.org/html/rfc7231#section-6.5.1",

                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
        }
    }
}
