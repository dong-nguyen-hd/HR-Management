using HR_Management.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR_Management.Data.Contexts.Config
{
    /// <summary>
    /// Setting schema for Location table
    /// </summary>
    public class LocationConfig : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> entity)
        {
            entity.ToTable("Location");
            entity.Property(x => x.Name).IsRequired().HasColumnType("nvarchar(500)");
            entity.Property(x => x.Address).IsRequired().HasColumnType("nvarchar(500)");
            entity.Property(x => x.Status).HasDefaultValue(true);
        }
    }
}
