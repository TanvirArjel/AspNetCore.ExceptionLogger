using ExceptionHandler.Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace ExceptionHandler.Demo.Data
{
    public class ExceptionHandlerDbContext : DbContext
    {
        public ExceptionHandlerDbContext(DbContextOptions<ExceptionHandlerDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>();
        }


    }
}
