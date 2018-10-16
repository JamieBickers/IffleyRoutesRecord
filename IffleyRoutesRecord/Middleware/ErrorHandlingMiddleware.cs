using IffleyRoutesRecord.Logic.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.Middleware
{
    /// <summary>
    /// Middleware to handle exceptions
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Invoke the next action in the pipeline
        /// </summary>
        /// <param name="context">Current http context</param>
        /// <returns>A task waiting on the rest of the pipeline</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code;

            switch (exception)
            {
                case EntityNotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
                case EntityWithNameAlreadyExistsException _:
                    code = HttpStatusCode.Conflict;
                    break;
                case InternalEntityNotFoundException _:
                    code = HttpStatusCode.InternalServerError;
                    break;
                case EntityCreationException _:
                    code = HttpStatusCode.InternalServerError;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }

            string result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
