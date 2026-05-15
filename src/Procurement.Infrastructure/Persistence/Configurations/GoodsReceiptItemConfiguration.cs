using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class GoodsReceiptItemConfiguration : IEntityTypeConfiguration<GoodsReceiptItem>
{
    public void Configure(EntityTypeBuilder<GoodsReceiptItem> builder)
    {
        builder.ToTable("GoodsReceiptItems");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.OrderedQuantity).HasPrecision(18, 4);
        builder.Property(i => i.ReceivedQuantity).HasPrecision(18, 4);

        builder.HasOne(i => i.ProcurementItem)
            .WithMany()
            .HasForeignKey(i => i.ProcurementItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
