using Business.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Config
{
    /// <summary>
    /// Setting schema for Pay table
    /// </summary>
    public class PayConfig : IEntityTypeConfiguration<Pay>
    {
        public void Configure(EntityTypeBuilder<Pay> entity)
        {
            entity.ToTable("Pay");
            entity.Property(x => x.Salary).HasColumnType("money");
            entity.Property(x => x.Allowance).HasColumnType("money");
            entity.Property(x => x.Bonus).HasColumnType("money");
            entity.Property(x => x.PIT).HasColumnType("money");
            entity.Property(x => x.SocialInsurance).HasColumnType("money");
            entity.Property(x => x.HealthInsurance).HasColumnType("money");
            entity.Property(x => x.Date).HasColumnType("date");
            entity.HasIndex(x => x.Date);
            entity.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
