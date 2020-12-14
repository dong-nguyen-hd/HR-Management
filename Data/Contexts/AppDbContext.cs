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

        // Use Fluent API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Education
            builder.Entity<Education>().ToTable("Education");
            builder.Entity<Education>().Property(x => x.CollegeName).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Education>().Property(x => x.Major).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Education>().Property(x => x.StartDate).IsRequired().HasColumnType("date");
            builder.Entity<Education>().Property(x => x.EndDate).HasColumnType("date");
            builder.Entity<Education>().Property(x => x.Status).HasDefaultValue(true);
            // WorkHistory
            builder.Entity<WorkHistory>().ToTable("WorkHistory");
            builder.Entity<WorkHistory>().Property(x => x.Position).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<WorkHistory>().Property(x => x.CompanyName).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<WorkHistory>().Property(x => x.StartDate).IsRequired().HasColumnType("date");
            builder.Entity<WorkHistory>().Property(x => x.EndDate).HasColumnType("date");
            builder.Entity<WorkHistory>().Property(x => x.Status).HasDefaultValue(true);
            // Certificate
            builder.Entity<Certificate>().ToTable("Certificate");
            builder.Entity<Certificate>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Certificate>().Property(x => x.Provider).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Certificate>().Property(x => x.StartDate).IsRequired().HasColumnType("date");
            builder.Entity<Certificate>().Property(x => x.EndDate).HasColumnType("date");
            builder.Entity<Certificate>().Property(x => x.Status).HasDefaultValue(true);
            // Location
            builder.Entity<Location>().ToTable("Location");
            builder.Entity<Location>().Property(x => x.Address).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Location>().Property(x => x.City).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Location>().Property(x => x.Country).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Location>().Property(x => x.Status).HasDefaultValue(true);
            // Project
            builder.Entity<Project>().ToTable("Project");
            builder.Entity<Project>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Project>().Property(x => x.Description).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Project>().Property(x => x.Position).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Project>().Property(x => x.Responsibilities).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Project>().Property(x => x.Technology).IsRequired().HasColumnType("varchar").HasMaxLength(255);
            builder.Entity<Project>().Property(x => x.StartDate).IsRequired().HasColumnType("date");
            builder.Entity<Project>().Property(x => x.EndDate).HasColumnType("date");
            builder.Entity<Project>().Property(x => x.Status).HasDefaultValue(true);
            // CategoryPerson
            builder.Entity<CategoryPerson>().ToTable("CategoryPerson");
            builder.Entity<CategoryPerson>().Property(x => x.Technology).IsRequired().HasColumnType("varchar").HasMaxLength(255);
            builder.Entity<CategoryPerson>().Property(x => x.Status).HasDefaultValue(true);
            // Category
            builder.Entity<Category>().ToTable("Category");
            builder.Entity<Category>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Category>().Property(x => x.Status).HasDefaultValue(true);
            // Technology
            builder.Entity<Technology>().ToTable("Technology");
            builder.Entity<Technology>().Property(x => x.Name).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Technology>().Property(x => x.Status).HasDefaultValue(true);
            // Person
            builder.Entity<Person>().ToTable("Person");
            builder.Entity<Person>().Property(x => x.FirstName).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Person>().Property(x => x.LastName).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Person>().Property(x => x.Email).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Person>().Property(x => x.Avatar).IsRequired().HasColumnType("varchar").HasMaxLength(255);
            builder.Entity<Person>().Property(x => x.Description).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Person>().Property(x => x.Phone).IsRequired().HasColumnType("varchar").HasMaxLength(25);
            builder.Entity<Person>().Property(x => x.CreatedBy).IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Person>().Property(x => x.UpdatedBy).IsRequired(false).HasColumnType("nvarchar").HasMaxLength(255);
            builder.Entity<Person>().Property(x => x.YearOfBirth).IsRequired().HasColumnType("date");
            builder.Entity<Person>().Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime2");
            builder.Entity<Person>().Property(x => x.UpdatedAt).IsRequired(false).HasColumnType("datetime2");
            builder.Entity<Person>().Property(x => x.OrderIndex).IsRequired().HasColumnType("varchar").HasMaxLength(255);
            builder.Entity<Person>().Property(x => x.StaffId).IsRequired().HasColumnType("varchar").HasMaxLength(255);
            builder.Entity<Person>().Property(x => x.Status).HasDefaultValue(true);
        }
    }
}
