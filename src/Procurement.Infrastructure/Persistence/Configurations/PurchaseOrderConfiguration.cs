using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.PoNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(p => p.PoNumber).IsUnique();

        builder.HasOne(p => p.ProcurementRequest)
            .WithMany()
            .HasForeignKey(p => p.ProcurementId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.GeneratedBy)
            .WithMany()
            .HasForeignKey(p => p.GeneratedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
