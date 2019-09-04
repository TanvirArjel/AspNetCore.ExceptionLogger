using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExceptionHandler.Demo.Data;
using ExceptionHandler.Demo.Extensions;

namespace ExceptionHandler.Demo
{
    public class DesignTimeDbContextFactory
    {
        public ExceptionHandlerDbContext CreateDbContext(string[] args)
        {
            var dbContextOptions = DbContextHelper.GetDbContextOptions();
            return new ExceptionHandlerDbContext(dbContextOptions);
        }
    }
}
