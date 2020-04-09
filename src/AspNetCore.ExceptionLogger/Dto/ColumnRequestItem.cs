// <copyright file="ColumnRequestItem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AspNetCore.ExceptionLogger.Dto
{
    public class ColumnRequestItem
    {
        public string Data { get; set; }

        public string Name { get; set; }

        public bool Searchable { get; set; }

        public bool Orderable { get; set; }

        public SearchRequestItem Search { get; set; }
    }
}
