// <copyright file="DataTableDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace AspNetCore.ExceptionLogger.Dto
{
    public class DataTableDto
    {
        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public List<ColumnRequestItem> Columns { get; set; }

        public List<OrderRequestItem> Order { get; set; }

        public SearchRequestItem Search { get; set; }
    }
}
