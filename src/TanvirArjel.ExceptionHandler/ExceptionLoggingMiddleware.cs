using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TanvirArjel.ExceptionHandler.Services;

namespace TanvirArjel.ExceptionHandler
{
    internal class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly PathString _redirectPath;
        
        public ExceptionLoggingMiddleware(RequestDelegate requestDelegate, PathString redirectPath)
        {
            _requestDelegate = requestDelegate;
            _redirectPath = redirectPath;
        }

        public async Task InvokeAsync(HttpContext context, IInternalExceptionService internalExceptionService)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception exception)
            {
                try
                {
                    await internalExceptionService.LogExceptionToDatabaseAsync(exception, context);
                    context.Response.Redirect(_redirectPath);
                }
                catch (Exception exception2)
                {
                    await context.Response.WriteAsync(exception2.Message + " " + exception2.StackTrace);
                }
                
                
            }

        }
    }
}
