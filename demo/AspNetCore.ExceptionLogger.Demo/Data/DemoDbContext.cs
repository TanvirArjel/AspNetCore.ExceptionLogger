using AspNetCore.ExceptionLogger.Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.ExceptionLogger.Demo.Data
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>();
        }


    }
}
