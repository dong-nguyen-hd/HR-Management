using HR_Management.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HR_Management.Data.Contexts.Config
{
    /// <summary>
    /// Setting schema for Account table
    /// </summary>
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> entity)
        {
            entity.ToTable("Account");
            entity.Property(x => x.UserName).IsRequired().HasColumnType("varchar(125)");
            entity.Property(x => x.Password).IsRequired().HasColumnType("varchar(125)");
            entity.Property(x => x.Name).IsRequired().HasColumnType("nvarchar(500)");
            entity.Property(x => x.Email).IsRequired().HasColumnType("nvarchar(500)");
            entity.Property(x => x.Role).IsRequired().HasColumnType("varchar(50)");
            entity.Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime2");
            entity.Property(x => x.LastActivity).IsRequired().HasColumnType("datetime2");
            entity.Property(x => x.Status).HasDefaultValue(true);

            // Seeding data for Account table
            entity.HasData(
                new Account
                {
                    Id = -1,
                    UserName = "admin",
                    Password = "10000.ej6YMBO1QrLxWx7AuxN8IA==.0CNmt0k0xfvshCGdRy8wTiapSrEWFedeozzNTakWkQw=", // Input: d7f32454b44d22182618d56e683f419a (MD5 hash)
                    Email = "dong.nguyen.hdkt@gmail.com",
                    Role = "admin",
                    CreatedAt = DateTime.UtcNow,
                    Name = "Dong Nguyen",
                    LastActivity = DateTime.UtcNow,
                    Status = true
                }
            );
        }
    }
}
