// <copyright file="IExceptionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCore.ExceptionLogger.Dto;

namespace AspNetCore.ExceptionLogger.Services
{
    /// <summary>
    /// Contains all the necessary methods to manipulate exceptions.
    /// </summary>
    public interface IExceptionService
    {
        /// <summary>
        /// This method takes <paramref name="dataTablesParams"/> which is type of jQuery DataTables post object and returns
        /// an object which can be bind to jQuery DataTables.
        /// </summary>
        /// <param name="dataTablesParams">jQuery DataTables Post object for server side rendering.</param>
        /// <returns>An object which can be bind to jQuery DataTables.</returns>
        Task<object> GetDataTableObject(DataTableDto dataTablesParams);

        /// <summary>
        /// This method takes <paramref name="exceptionId"/> as input param and returns the details of the exception
        /// of the provided id.
        /// </summary>
        /// <param name="exceptionId">A <see cref="long"/> positive number.</param>
        /// <returns>Returns <see cref="ExceptionModel"/> object.</returns>
        Task<ExceptionModel> GetExceptionAsync(long exceptionId);

        /// <summary>
        /// This method takes <paramref name="exceptionId"/> as input param and change the exception status from NotFixed to Fixed.
        /// </summary>
        /// <param name="exceptionId">A <see cref="long"/> positive number.</param>
        /// <returns>Returns <see cref="Task"/></returns>
        Task MarkExceptionAsFixedAsync(long exceptionId);

        /// <summary>
        /// This method takes the <paramref name="exceptionId"/> as input param and delete the exception entity from the database.
        /// </summary>
        /// <param name="exceptionId">A <see cref="long"/> positive number.</param>
        /// <returns>Returns <see cref="Task"/></returns>
        Task DeleteExceptionAsync(long exceptionId);

        /// <summary>
        /// This method takes a list of exception ids as input param and delete all the exception entity from the database of the
        /// provided ids.
        /// </summary>
        /// <param name="exceptionIds">A list of <see cref="long"/> positive number.</param>
        /// <returns>Returns <see cref="Task"/></returns>
        Task DeleteExceptionsAsync(List<long> exceptionIds);
    }
}
