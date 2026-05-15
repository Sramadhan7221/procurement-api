using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class ProcurementRequestConfiguration : IEntityTypeConfiguration<ProcurementRequest>
{
    public void Configure(EntityTypeBuilder<ProcurementRequest> builder)
    {
        builder.ToTable("ProcurementRequests");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Title).IsRequired().HasMaxLength(200);
        builder.Property(r => r.Description).HasMaxLength(1000);
        builder.Property(r => r.TotalPrice).HasPrecision(18, 2);
        builder.Property(r => r.Status).HasConversion<int>();
        builder.Property(r => r.ManagerComment).HasMaxLength(1000);
        builder.Property(r => r.AdminComment).HasMaxLength(1000);

        builder.HasOne(r => r.CreatedBy)
            .WithMany(u => u.ProcurementRequests)
            .HasForeignKey(r => r.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ManagerReviewedBy)
            .WithMany()
            .HasForeignKey(r => r.ManagerReviewedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.AdminReviewedBy)
            .WithMany()
            .HasForeignKey(r => r.AdminReviewedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Items)
            .WithOne(i => i.ProcurementRequest)
            .HasForeignKey(i => i.ProcurementRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(r => r.Invoice).IsRequired(false);
    }
}
