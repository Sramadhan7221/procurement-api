using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class ProcurementItemConfiguration : IEntityTypeConfiguration<ProcurementItem>
{
    public void Configure(EntityTypeBuilder<ProcurementItem> builder)
    {
        builder.ToTable("ProcurementItems");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.UoM).IsRequired().HasMaxLength(50);
        builder.Property(i => i.ItemName).IsRequired().HasMaxLength(200);
        builder.Property(i => i.UnitPrice).HasPrecision(18, 2);
        builder.Ignore(i => i.SubTotal);
    }
}
