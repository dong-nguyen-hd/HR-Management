using Business.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Config
{
    /// <summary>
    /// Setting schema for CategoryPerson table
    /// </summary>
    public class CategoryPersonConfig : IEntityTypeConfiguration<CategoryPerson>
    {
        public void Configure(EntityTypeBuilder<CategoryPerson> entity)
        {
            entity.ToTable("CategoryPerson");
            entity.Property(x => x.Technology).IsRequired().HasColumnType("varchar(max)");
            entity.Property(x => x.Status).HasDefaultValue(true);
        }
    }
}
