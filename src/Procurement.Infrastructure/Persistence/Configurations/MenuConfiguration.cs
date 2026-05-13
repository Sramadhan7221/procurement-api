using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Description).HasMaxLength(500);
        builder.Property(m => m.Url).HasMaxLength(200);
        builder.Property(m => m.Icon).HasMaxLength(100);
        builder.Property(m => m.Sequence).IsRequired();

        builder.HasOne(m => m.ParentMenu)
            .WithMany(m => m.Children)
            .HasForeignKey(m => m.ParentMenuId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
