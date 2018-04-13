using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using VehicleMonitoring.Common.Core.Exceptions;

namespace VehicleMonitoring.Common.Core.FilterAttributes
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ApiError apiError = null;
            if (context.Exception is ApiException)
            {
                // handle explicit 'known' API errors
                var ex = context.Exception as ApiException;
                context.Exception = null;
                apiError = new ApiError(ex.Message)
                {
                    Errors = ex.Errors
                };

                context.HttpContext.Response.StatusCode = ex.StatusCode;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;
            }
            else
            {
                #if !DEBUG
                var msg = "An unhandled error occurred.";                
                string stack = null;
                #else
                var msg = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;
#endif

                apiError = new ApiError(msg)
                {
                    detail = stack
                };

                context.HttpContext.Response.StatusCode = 500;
            }

            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }
    }
}
