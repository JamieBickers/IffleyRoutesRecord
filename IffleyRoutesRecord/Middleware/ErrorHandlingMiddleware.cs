using IffleyRoutesRecord.Logic.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

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
                case EntityNotFoundException entityNotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case EntityWithNameAlreadyExistsException entityWithNameAlreadyExistsException:
                    code = HttpStatusCode.Conflict;
                    break;
                case InternalEntityNotFoundException internalEntityNotFoundException:
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
