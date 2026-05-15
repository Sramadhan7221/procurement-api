using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class ProcurementInvoiceItemConfiguration : IEntityTypeConfiguration<ProcurementInvoiceItem>
{
    public void Configure(EntityTypeBuilder<ProcurementInvoiceItem> builder)
    {
        builder.ToTable("ProcurementInvoiceItems");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.InvoicedQuantity).HasPrecision(18, 4);
        builder.Property(i => i.InvoiceUnitPrice).HasPrecision(18, 2);

        builder.HasOne(i => i.ProcurementItem)
            .WithMany()
            .HasForeignKey(i => i.ProcurementItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
