using Business.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Config
{
    /// <summary>
    /// Setting schema for Office table
    /// </summary>
    public class OfficeConfig : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> entity)
        {
            entity.ToTable("Office");
            entity.Property(x => x.Name).IsRequired().HasColumnType("nvarchar(500)");
            entity.Property(x => x.Address).IsRequired().HasColumnType("nvarchar(500)");
            entity.HasIndex(x => x.Name);
            entity.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
