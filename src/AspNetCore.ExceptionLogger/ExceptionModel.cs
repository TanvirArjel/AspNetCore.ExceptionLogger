// <copyright file="ExceptionModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AspNetCore.ExceptionLogger
{
    /// <summary>
    /// Model class which will represent Exceptions table in the database.
    /// </summary>
    [Table("Exceptions")]
    public class ExceptionModel
    {
        /// <summary>
        /// Primary key of the Exceptions table.
        /// </summary>
        [Key]
        public long ExceptionId { get; set; }

        /// <summary>
        /// The UTC time at which the exception has been occured.
        /// </summary>
        public DateTime ExceptionTime { get; set; }

        /// <summary>
        /// The requested http url when the exception was occured.
        /// </summary>
        public string RequestedUrl { get; set; }

        /// <summary>
        /// The requested Controller name  when the exception was occured.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// The requested Controller acton or method name  when the exception was occured.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// The exception message.
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// The inner exception message.
        /// </summary>
        public string InnerExceptionMessage { get; set; }

        /// <summary>
        /// The StackTrace of the exception occured.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// The submitted form data if the request is a POST request.
        /// </summary>
        public string FormData { get; set; }

        /// <summary>
        /// The state of the exception.
        /// </summary>
        public bool IsFixed { get; set; }
    }
}
