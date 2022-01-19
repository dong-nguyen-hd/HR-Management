using Business.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Infrastructure.Contexts.Config
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
                    Password = "10000.YNMP1rFAoc8L+0I4UeerFg==.qVRWc4jYxx/BMt8ro9rjNyr8nxgvClamDkef+j7vLE8=", // Input: 21232f297a57a5a743894a0e4a801fc3 (plain-text: admin) (hash function: MD5)
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
