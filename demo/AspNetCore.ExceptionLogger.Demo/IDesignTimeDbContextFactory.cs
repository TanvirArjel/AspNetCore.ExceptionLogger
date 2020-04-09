using AspNetCore.ExceptionLogger.Demo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.ExceptionLogger.Demo
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DemoDbContext>
    {
        public DemoDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DemoDbContext>();
            var connectionString = configuration.GetConnectionString("LocalDbConnection");
            builder.UseSqlServer(connectionString);
            return new DemoDbContext(builder.Options);
        }
    }
}
