using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("Vendors");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Name).IsRequired().HasMaxLength(200);
        builder.Property(v => v.ContactEmail).IsRequired().HasMaxLength(200);
        builder.Property(v => v.ContactPhone).IsRequired().HasMaxLength(50);
        builder.Property(v => v.Address).IsRequired().HasMaxLength(500);
        builder.Property(v => v.CreatedAt).IsRequired();
        builder.Property(v => v.UpdatedAt);
        builder.Property(v => v.IsDeleted).HasDefaultValue(false);
        builder.Property(v => v.DeletedAt);
        builder.HasQueryFilter(v => !v.IsDeleted);
    }
}
