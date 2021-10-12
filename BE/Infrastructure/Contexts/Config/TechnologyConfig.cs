using Business.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Config
{
    /// <summary>
    /// Setting schema for Technology table
    /// </summary>
    public class TechnologyConfig : IEntityTypeConfiguration<Technology>
    {
        public void Configure(EntityTypeBuilder<Technology> entity)
        {
            entity.ToTable("Technology");
            entity.Property(x => x.Name).IsRequired().HasColumnType("nvarchar(500)");
            entity.Property(x => x.Status).HasDefaultValue(true);
        }
    }
}
