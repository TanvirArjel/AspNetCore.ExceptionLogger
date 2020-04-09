// <copyright file="ExceptionLoggingMiddleware.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using AspNetCore.ExceptionLogger.Services;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.ExceptionLogger
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
                await _requestDelegate(context).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                try
                {
                    await internalExceptionService.LogExceptionToDatabaseAsync(exception, context).ConfigureAwait(false);
                    context.Response.Redirect(_redirectPath);
                }
                catch (Exception exception2)
                {
                    await context.Response.WriteAsync(exception2.Message + " " + exception2.StackTrace).ConfigureAwait(false);
                }
            }
        }
    }
}
