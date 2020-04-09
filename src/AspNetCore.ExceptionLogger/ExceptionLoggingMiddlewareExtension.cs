// <copyright file="ExceptionLoggingMiddlewareExtension.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.ExceptionLogger
{
    /// <summary>
    /// Contains all the exception handling middleware extension methods.
    /// </summary>
    public static class ExceptionLoggingMiddlewareExtension
    {
        /// <summary>
        /// Adds a middleware to the pipeline that will catch exceptions, log them, reset the request path, and re-execute the request.
        /// The request will not be re-executed if the response has already started.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance that this method extends.</param>
        /// <param name="redirectPath">A redirect <see cref="PathString"/> to which request will be redirected after handling the exception.</param>
        /// <exception cref="ArgumentNullException"> Thrown if <paramref name="redirectPath"/> is null.</exception>
        /// <returns>Returns <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseExceptionLogger(
            this IApplicationBuilder builder, PathString redirectPath)
        {
            if (redirectPath == null)
            {
                throw new ArgumentNullException(nameof(redirectPath));
            }

            return builder.UseMiddleware<ExceptionLoggingMiddleware>(redirectPath);
        }
    }
}
