using ChatAPI.BLL.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.PL
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            Console.WriteLine("Exception occurred: {0}", exception.Message);

            ProblemDetails problemDetails = new();

            switch (exception)
            {
                case NotFoundException:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Not found";
                    break;
                case BadRequestException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Bad request";
                    break;
                default:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Internal server error";
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}