// <copyright file="ExceptionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using AspNetCore.ExceptionLogger.Dto;
using AspNetCore.ExceptionLogger.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace AspNetCore.ExceptionLogger.Services
{
    internal class ExceptionService : IExceptionService, IInternalExceptionService
    {
        private readonly string _connectionString;

        public ExceptionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Not applicable here")]
        public async Task<object> GetDataTableObject(DataTableDto dataTablesParams)
        {
            int draw = dataTablesParams.Draw;
            int start = dataTablesParams.Start;
            int length = dataTablesParams.Length;

            // Sorting Column and order
            string sortColumnName = dataTablesParams.Columns[dataTablesParams.Order[0].Column].Name;
            string sortColumnDir = dataTablesParams.Order[0].Dir;

            // Individual Column Search value
            string isFixed = dataTablesParams.Columns[2].Search.Value?.ToUpperInvariant();
            string exceptionTime = dataTablesParams.Columns[3].Search.Value?.ToUpperInvariant();
            string requestedUrl = dataTablesParams.Columns[4].Search.Value?.ToUpperInvariant();
            string controllerName = dataTablesParams.Columns[5].Search.Value?.ToUpperInvariant();

            string actionName = dataTablesParams.Columns[6].Search.Value?.ToUpperInvariant();
            string exceptionMessage = dataTablesParams.Columns[7].Search.Value?.ToUpperInvariant();
            string innerExceptionMessage = dataTablesParams.Columns[8].Search.Value?.ToUpperInvariant();

            // Global Search
            string globalSearchValue = dataTablesParams.Search.Value?.ToUpperInvariant();

            long totalRecords = 0;
            long totalFilteredRecords = 0;
            List<ExceptionModel> exceptionList = new List<ExceptionModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlCountString = $"SELECT COUNT(ExceptionId) FROM dbo.Exceptions";

                using (SqlCommand sqlCommand = new SqlCommand(sqlCountString, connection))
                {
                    connection.Open();
                    totalRecords = (int)await sqlCommand.ExecuteScalarAsync().ConfigureAwait(false);
                    connection.Close();
                }

                var mainSqlQueryString = "SELECT * FROM dbo.Exceptions";

                var globalSearchQueryString = string.Empty;
                var individualColumnSearchString = string.Empty;

                // Global searching implementation
                if (!string.IsNullOrWhiteSpace(globalSearchValue))
                {
                    globalSearchQueryString = $"Where RequestedUrl LIKE '%{globalSearchValue}%' OR ControllerName LIKE '%{globalSearchValue}%'" +
                                         $" OR ActionName LIKE '%{globalSearchValue}%' OR ExceptionMessage LIKE '%{globalSearchValue}%' OR InnerExceptionMessage LIKE '%{globalSearchValue}%'";

                    mainSqlQueryString = $"{mainSqlQueryString} {globalSearchQueryString}";
                }

                // Searching By Individual Column
                if (!string.IsNullOrWhiteSpace(requestedUrl) || !string.IsNullOrWhiteSpace(controllerName) || !string.IsNullOrWhiteSpace(actionName) ||
                    !string.IsNullOrWhiteSpace(exceptionMessage)
                    || !string.IsNullOrWhiteSpace(exceptionTime) || !string.IsNullOrWhiteSpace(innerExceptionMessage) ||
                    !string.IsNullOrWhiteSpace(isFixed))
                {
                    individualColumnSearchString = $"{(string.IsNullOrWhiteSpace(globalSearchValue) ? "Where" : "AND")} RequestedUrl LIKE '%{requestedUrl}%' AND ControllerName LIKE '%{controllerName}%'" +
                                                   $" AND ActionName LIKE '%{actionName}%' AND ExceptionMessage LIKE '%{exceptionMessage}%' AND InnerExceptionMessage LIKE '%{innerExceptionMessage}%'";

                    mainSqlQueryString = $"{mainSqlQueryString} {individualColumnSearchString}";
                }

                // Filtered row count
                string sqlFilteredCountString = $"{sqlCountString} {globalSearchQueryString} {individualColumnSearchString}";
                using (SqlCommand sqlCommand = new SqlCommand(sqlFilteredCountString, connection))
                {
                    connection.Open();
                    totalFilteredRecords = (int)await sqlCommand.ExecuteScalarAsync().ConfigureAwait(false);
                    connection.Close();
                }

                // Sorting By Individual Column
                if (!(string.IsNullOrWhiteSpace(sortColumnName) && string.IsNullOrWhiteSpace(sortColumnDir)))
                {
                    mainSqlQueryString = $"{mainSqlQueryString} ORDER BY {sortColumnName} {sortColumnDir}";
                }

                // Skip and Take implementation
                mainSqlQueryString = $"{mainSqlQueryString} OFFSET {start} ROWS FETCH NEXT {length} ROWS ONLY";

                using (SqlCommand sqlCommand = new SqlCommand(mainSqlQueryString, connection))
                {
                    connection.Open();
                    using (SqlDataReader sdr = await sqlCommand.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await sdr.ReadAsync().ConfigureAwait(false))
                        {
                            ExceptionModel exception = sdr.ConvertToObject<ExceptionModel>();
                            exceptionList.Add(exception);
                        }
                    }
                }
            }

            return new
            {
                draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalFilteredRecords,
                data = exceptionList
            };
        }

        public async Task<ExceptionModel> GetExceptionAsync(long exceptionId)
        {
            ExceptionModel exception = new ExceptionModel();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlCommandText = "SELECT * FROM dbo.Exceptions WHERE ExceptionId = @exceptionId";

                using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, connection))
                {
                    sqlCommand.Parameters.Add("@exceptionId", SqlDbType.BigInt).Value = exceptionId;
                    connection.Open();
                    using (SqlDataReader sdr = await sqlCommand.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        if (await sdr.ReadAsync().ConfigureAwait(false))
                        {
                            exception = sdr.ConvertToObject<ExceptionModel>();
                        }
                    }
                }
            }

            return exception;
        }

        public async Task MarkExceptionAsFixedAsync(long exceptionId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlCommandText = "UPDATE dbo.Exceptions SET IsFixed = 1 WHERE ExceptionId = @exceptionId";

                using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, connection))
                {
                    sqlCommand.Parameters.Add("@exceptionId", SqlDbType.BigInt).Value = exceptionId;
                    connection.Open();
                    await sqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task DeleteExceptionAsync(long exceptionId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlString = "DELETE FROM dbo.Exceptions WHERE ExceptionId = @exceptionId";

                using (SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
                {
                    sqlCommand.Parameters.Add("@exceptionId", SqlDbType.BigInt).Value = exceptionId;
                    connection.Open();
                    await sqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task DeleteExceptionsAsync(List<long> exceptionIds)
        {
            var exceptionToBeDeleted = string.Join(",", exceptionIds);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlString = "DELETE FROM dbo.Exceptions WHERE ExceptionId IN (@exceptionToBeDeleted)";

                using (SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
                {
                    sqlCommand.Parameters.Add("@exceptionToBeDeleted", SqlDbType.BigInt).Value = exceptionToBeDeleted;
                    connection.Open();
                    await sqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task LogExceptionToDatabaseAsync(Exception exception, HttpContext context)
        {
            try
            {
                string controllerName = context.GetRouteValue("controller").ToString();
                string actionName = context.GetRouteData().Values["action"].ToString();
                string exceptionMessage = exception.Message;
                string innerExceptionMessage = exception.InnerException?.InnerException?.Message ?? exception.InnerException?.Message ?? "No inner exception";
                string stackTrace = exception.StackTrace;
                var requestedUrl = context.Request.GetDisplayUrl();
                string formData = null;

                try
                {
                    if (context.Request.Form != null)
                    {
                        var formKeys = context.Request.Form.Keys;
                        Dictionary<string, object> postedModelDict = new Dictionary<string, object>();
                        foreach (string key in formKeys)
                        {
                            if (key != "__RequestVerificationToken")
                            {
                                context.Request.Form.TryGetValue(key, out StringValues stringValues);
                                postedModelDict.Add(key, stringValues.ToString());
                            }
                        }

                        formData = JsonConvert.SerializeObject(postedModelDict);
                    }
                }
                catch (Exception /*exception*/)
                {
                    // Ignore
                }

                var query = context.Request.Query;
                var queryString = context.Request.QueryString;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlString = "INSERT INTO dbo.Exceptions(ExceptionTime,RequestedUrl,ControllerName,ActionName,ExceptionMessage,InnerExceptionMessage,StackTrace,FormData,IsFixed) VALUES(getutcdate(),@requestedUrl,@controllerName,@actionName,@exceptionMessage,@innerExceptionMessage,@stackTrace,@formData,0)";

                    using (SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
                    {
                        sqlCommand.Parameters.Add("requestedUrl", SqlDbType.NVarChar).Value = requestedUrl;
                        sqlCommand.Parameters.Add("controllerName", SqlDbType.NVarChar).Value = controllerName;
                        sqlCommand.Parameters.Add("actionName", SqlDbType.NVarChar).Value = actionName;
                        sqlCommand.Parameters.Add("exceptionMessage", SqlDbType.NVarChar).Value = exceptionMessage;
                        sqlCommand.Parameters.Add("innerExceptionMessage", SqlDbType.NVarChar).Value = innerExceptionMessage;
                        sqlCommand.Parameters.Add("stackTrace", SqlDbType.NVarChar).Value = stackTrace;
                        sqlCommand.Parameters.Add("formData", SqlDbType.NVarChar).Value = (object)formData ?? DBNull.Value;

                        connection.Open();
                        await sqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception finalException)
            {
                Console.Write(finalException);
                throw;
            }
        }
    }
}
