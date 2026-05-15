using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class ProcurementPaymentConfiguration : IEntityTypeConfiguration<ProcurementPayment>
{
    public void Configure(EntityTypeBuilder<ProcurementPayment> builder)
    {
        builder.ToTable("ProcurementPayments");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.PaymentReference).HasMaxLength(200);
        builder.Property(p => p.PaymentProofFilePath).HasMaxLength(500);

        builder.HasOne(p => p.ProcurementRequest)
            .WithMany()
            .HasForeignKey(p => p.ProcurementId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Invoice)
            .WithMany()
            .HasForeignKey(p => p.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.ApprovedBy)
            .WithMany()
            .HasForeignKey(p => p.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.PaidBy)
            .WithMany()
            .HasForeignKey(p => p.PaidByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
