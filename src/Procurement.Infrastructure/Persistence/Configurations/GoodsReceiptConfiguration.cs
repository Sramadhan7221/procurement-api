using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodsReceipt>
{
    public void Configure(EntityTypeBuilder<GoodsReceipt> builder)
    {
        builder.ToTable("GoodsReceipts");
        builder.HasKey(gr => gr.Id);
        builder.Property(gr => gr.Notes).HasMaxLength(1000);

        builder.HasOne(gr => gr.ProcurementRequest)
            .WithMany()
            .HasForeignKey(gr => gr.ProcurementId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(gr => gr.ReceivedBy)
            .WithMany()
            .HasForeignKey(gr => gr.ReceivedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(gr => gr.Items)
            .WithOne(i => i.GoodsReceipt)
            .HasForeignKey(i => i.GoodsReceiptId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
