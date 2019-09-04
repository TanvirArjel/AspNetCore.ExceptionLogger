using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExceptionHandler.Demo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExceptionHandler.Demo.Extensions
{
    public static class DbContextHelper
    {
        public static DbContextOptions<ExceptionHandlerDbContext> GetDbContextOptions()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("LocalDbConnection");

            var options = new DbContextOptionsBuilder<ExceptionHandlerDbContext>()
                .UseSqlServer(new SqlConnection(connectionString)).Options;

            return options;

        }
    }
}
