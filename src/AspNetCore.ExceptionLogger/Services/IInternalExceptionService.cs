// <copyright file="IInternalExceptionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.ExceptionLogger.Services
{
    /// <summary>
    /// Contain all the method to manipulate exceptions within this class library project only.
    /// </summary>
    internal interface IInternalExceptionService
    {
        /// <summary>
        /// This method takes <paramref name="exception"/> and <see cref="context"/> as input params and log the exception into database.
        /// </summary>
        /// <param name="exception">An <see cref="Exception"/> object.</param>
        /// <param name="context">A <see cref="HttpContext"/> object.</param>
        /// <returns>Returns <see cref="Task"/></returns>
        Task LogExceptionToDatabaseAsync(Exception exception, HttpContext context);
    }
}
