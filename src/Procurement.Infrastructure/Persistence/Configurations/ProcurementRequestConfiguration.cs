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

        builder.HasOne(r => r.CreatedBy)
            .WithMany(u => u.ProcurementRequests)
            .HasForeignKey(r => r.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Items)
            .WithOne(i => i.ProcurementRequest)
            .HasForeignKey(i => i.ProcurementRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
