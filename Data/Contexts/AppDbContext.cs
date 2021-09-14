using HR_Management.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HR_Management.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Education> Educations { get; set; }
        public DbSet<WorkHistory> WorkHistories { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<CategoryPerson> CategoryPersons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Account> Accounts { get; set; }

        // Use Fluent API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Finds and runs all your configuration classes in the same assembly as the DbContext
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
