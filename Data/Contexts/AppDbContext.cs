using HR_Management.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HR_Management.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Education> Educations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<WorkHistory> WorkHistories { get; set; }


        // Use Fluent API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Category
            builder.Entity<Category>().ToTable("Category");
            // Certificate
            builder.Entity<Certificate>().ToTable("Certificate");
            // Education
            builder.Entity<Education>().ToTable("Education");
            // Person
            builder.Entity<Person>().ToTable("Person");
            // Project
            builder.Entity<Project>().ToTable("Project");
            // Technology
            builder.Entity<Technology>().ToTable("Technology");
            // WorkHistory
            builder.Entity<WorkHistory>().ToTable("WorkHistory");
        }
    }
}
