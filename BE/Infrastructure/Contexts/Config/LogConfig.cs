using Business.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Config
{
    /// <summary>
    /// Setting schema for Log table
    /// </summary>
    public class LogConfig : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> entity)
        {
            entity.ToTable("Log");
            entity.Property(x => x.UpdatedAt).HasColumnType("datetime2");
            entity.Property(x => x.UpdatedBy).IsRequired().HasColumnType("nvarchar(500)");
            entity.Property(x => x.Perpose).IsRequired().HasColumnType("nvarchar(500)");
        }
    }
}
