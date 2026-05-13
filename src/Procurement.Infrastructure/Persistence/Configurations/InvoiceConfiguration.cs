using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(i => i.InvoiceNumber)
            .IsUnique();

        builder.Property(i => i.Amount)
            .HasPrecision(18, 2);

        builder.Property(i => i.PaymentDate)
            .IsRequired();

        builder.Property(i => i.Notes)
            .HasMaxLength(1000);

        builder.Property(i => i.AttachmentPath)
            .HasMaxLength(500);

        builder.HasOne(i => i.ProcurementRequest)
            .WithOne(r => r.Invoice)
            .HasForeignKey<Invoice>(i => i.ProcurementRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
