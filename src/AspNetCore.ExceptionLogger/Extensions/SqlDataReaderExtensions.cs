// <copyright file="SqlDataReaderExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Data.SqlClient;
using System.Linq;
using FastMember;

namespace AspNetCore.ExceptionLogger.Extensions
{
    /// <summary>
    /// Contains all the extension methods of the <see cref="SqlDataReader"/>.
    /// </summary>
    internal static class SqlDataReaderExtensions
    {
        /// <summary>
        /// This method is used to convert an <see cref="SqlDataReader"/> instance to an C# object.
        /// </summary>
        /// <typeparam name="T">Type of which the <see cref="SqlDataReader"/> instance will be converted to.</typeparam>
        /// <param name="rd">The <see cref="SqlDataReader"/> instance which this method extends.</param>
        /// <returns>Returns the <typeparamref name="T"/></returns>
        internal static T ConvertToObject<T>(this SqlDataReader rd)
            where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[t, fieldName] = rd.GetValue(i);
                    }
                }
            }

            return t;
        }
    }
}
