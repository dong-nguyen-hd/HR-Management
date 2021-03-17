using HR_Management.Domain.Models;
using Microsoft.EntityFrameworkCore;

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

        // Use Fluent API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Education
            builder.Entity<Education>().ToTable("Education");
            builder.Entity<Education>().Property(x => x.CollegeName).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Education>().Property(x => x.Major).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Education>().Property(x => x.StartDate).IsRequired().HasColumnType("date");
            builder.Entity<Education>().Property(x => x.EndDate).HasColumnType("date");
            builder.Entity<Education>().Property(x => x.Status).HasDefaultValue(true);
            // WorkHistory
            builder.Entity<WorkHistory>().ToTable("WorkHistory");
            builder.Entity<WorkHistory>().Property(x => x.Position).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<WorkHistory>().Property(x => x.CompanyName).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<WorkHistory>().Property(x => x.StartDate).IsRequired().HasColumnType("date");
            builder.Entity<WorkHistory>().Property(x => x.EndDate).HasColumnType("date");
            builder.Entity<WorkHistory>().Property(x => x.Status).HasDefaultValue(true);
            // Certificate
            builder.Entity<Certificate>().ToTable("Certificate");
            builder.Entity<Certificate>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Certificate>().Property(x => x.Provider).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Certificate>().Property(x => x.StartDate).IsRequired().HasColumnType("date");
            builder.Entity<Certificate>().Property(x => x.EndDate).HasColumnType("date");
            builder.Entity<Certificate>().Property(x => x.Status).HasDefaultValue(true);
            // Location
            builder.Entity<Location>().ToTable("Location");
            builder.Entity<Location>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Location>().Property(x => x.Address).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Location>().Property(x => x.Status).HasDefaultValue(true);
            // Project
            builder.Entity<Project>().ToTable("Project");
            builder.Entity<Project>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Project>().Property(x => x.Description).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Project>().Property(x => x.Position).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Project>().Property(x => x.Responsibilities).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Project>().Property(x => x.Technology).IsRequired().HasColumnType("varchar(max)");
            builder.Entity<Project>().Property(x => x.StartDate).IsRequired().HasColumnType("date");
            builder.Entity<Project>().Property(x => x.EndDate).HasColumnType("date");
            builder.Entity<Project>().Property(x => x.Status).HasDefaultValue(true);
            // CategoryPerson
            builder.Entity<CategoryPerson>().ToTable("CategoryPerson");
            builder.Entity<CategoryPerson>().Property(x => x.Technology).IsRequired().HasColumnType("varchar(max)");
            builder.Entity<CategoryPerson>().Property(x => x.Status).HasDefaultValue(true);
            // Category
            builder.Entity<Category>().ToTable("Category");
            builder.Entity<Category>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Category>().Property(x => x.Status).HasDefaultValue(true);
            // Technology
            builder.Entity<Technology>().ToTable("Technology");
            builder.Entity<Technology>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Technology>().Property(x => x.Status).HasDefaultValue(true);
            // Person
            builder.Entity<Person>().ToTable("Person");
            builder.Entity<Person>().Property(x => x.FirstName).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Person>().Property(x => x.LastName).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Person>().Property(x => x.Email).HasColumnType("nvarchar(500)");
            builder.Entity<Person>().Property(x => x.Avatar).HasColumnType("varchar(250)");
            builder.Entity<Person>().Property(x => x.Description).HasColumnType("nvarchar(500)");
            builder.Entity<Person>().Property(x => x.Phone).HasColumnType("varchar(25)");
            builder.Entity<Person>().Property(x => x.CreatedBy).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Person>().Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime2");
            builder.Entity<Person>().Property(x => x.YearOfBirth).IsRequired().HasColumnType("date");
            builder.Entity<Person>().Property(x => x.OrderIndex).IsRequired().HasColumnType("varchar(250)");
            builder.Entity<Person>().Property(x => x.StaffId).IsRequired().HasColumnType("varchar(25)");
            builder.Entity<Person>().Property(x => x.Status).HasDefaultValue(true);
            // Log
            builder.Entity<Log>().ToTable("Log");
            builder.Entity<Log>().Property(x => x.UpdatedAt).HasColumnType("datetime2");
            builder.Entity<Log>().Property(x => x.UpdatedBy).IsRequired().HasColumnType("nvarchar(500)");
            builder.Entity<Log>().Property(x => x.Perpose).IsRequired().HasColumnType("nvarchar(500)");
        }
    }
}
