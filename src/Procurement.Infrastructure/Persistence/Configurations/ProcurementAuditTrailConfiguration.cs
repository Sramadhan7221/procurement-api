using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class ProcurementAuditTrailConfiguration : IEntityTypeConfiguration<ProcurementAuditTrail>
{
    public void Configure(EntityTypeBuilder<ProcurementAuditTrail> builder)
    {
        builder.ToTable("ProcurementAuditTrails");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.ActorName).IsRequired().HasMaxLength(200);
        builder.Property(a => a.ActorRole).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Action).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Comment).HasMaxLength(1000);
        builder.Property(a => a.Metadata).HasColumnType("text");
        builder.Property(a => a.FromStatus).HasConversion<int?>();
        builder.Property(a => a.ToStatus).HasConversion<int>();

        builder.HasOne(a => a.ProcurementRequest)
            .WithMany()
            .HasForeignKey(a => a.ProcurementId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
