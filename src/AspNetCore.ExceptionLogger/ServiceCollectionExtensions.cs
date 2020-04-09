// <copyright file="ServiceCollectionExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Data.SqlClient;
using AspNetCore.ExceptionLogger.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.ExceptionLogger
{
    /// <summary>
    /// Contain all the service collection extension methods for TanvirArjelExceptionHandler.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register all the necessary TanvirArjelExceptionHandler services to the ASP.NET Core Dependency Injection container
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance this method extends.</param>
        /// <param name="connectionString">Connection String of the <c>SQL Server</c> database to which exception will logged.</param>
        /// <exception cref="ArgumentNullException"> Thrown if <c>SQL Server</c> connection string is null or empty.</exception>
        public static void RegisterTanvirArjelExceptionHandler(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            CreateExceptionTable(connectionString);

            services.AddScoped<IInternalExceptionService, ExceptionService>(sp => new ExceptionService(connectionString));
            services.AddScoped<IExceptionService, ExceptionService>(sp => new ExceptionService(connectionString));
        }

        /// <summary>
        /// This method creates the table to which exceptions will be logged.
        /// </summary>
        /// <param name="connectionString">Connection String of the <c>SQL Server</c> database to which exception will logged.</param>
        /// /// <exception cref="ArgumentNullException"> Thrown if <c>SQL Server</c> connection string is null or empty.</exception>
        private static void CreateExceptionTable(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            using (var connection = new SqlConnection(connectionString))
            {
                string sqlString = $"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Exceptions' AND TABLE_SCHEMA = 'dbo'";

                using (SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
                {
                    connection.Open();
                    using (SqlDataReader sdr = sqlCommand.ExecuteReader())
                    {
                        if (!sdr.HasRows)
                        {
                            connection.Close();
                            sqlCommand.CommandText = "CREATE TABLE Exceptions (ExceptionId bigint IDENTITY(1,1) PRIMARY KEY,ExceptionTime datetime2,RequestedUrl nvarchar(max),ControllerName nvarchar(255)," +
                                                     "ActionName nvarchar(255), ExceptionMessage nvarchar(max),InnerExceptionMessage nvarchar(max),StackTrace nvarchar(max),FormData nvarchar(max),IsFixed bit);";
                            connection.Open();
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
