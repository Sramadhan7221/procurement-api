using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class ProcurementInvoiceConfiguration : IEntityTypeConfiguration<ProcurementInvoice>
{
    public void Configure(EntityTypeBuilder<ProcurementInvoice> builder)
    {
        builder.ToTable("ProcurementInvoices");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.VendorInvoiceNumber).IsRequired().HasMaxLength(100);
        builder.Property(i => i.FilePath).IsRequired().HasMaxLength(500);
        builder.Property(i => i.DisputeNote).HasMaxLength(1000);
        builder.Property(i => i.MatchingStatus).HasConversion<int>();

        builder.HasOne(i => i.ProcurementRequest)
            .WithMany()
            .HasForeignKey(i => i.ProcurementId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.UploadedBy)
            .WithMany()
            .HasForeignKey(i => i.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.VerifiedBy)
            .WithMany()
            .HasForeignKey(i => i.VerifiedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(i => i.Items)
            .WithOne(ii => ii.Invoice)
            .HasForeignKey(ii => ii.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
