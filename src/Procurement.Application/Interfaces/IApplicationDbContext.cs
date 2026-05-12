using Microsoft.EntityFrameworkCore;
using Procurement.Domain.Entities;

namespace Procurement.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Role> Roles { get; }
    DbSet<User> Users { get; }
    DbSet<ProcurementRequest> ProcurementRequests { get; }
    DbSet<ProcurementItem> ProcurementItems { get; }
    DbSet<Category> Categories { get; }
    DbSet<Division> Divisions { get; }
    DbSet<Product> Products { get; }
    DbSet<Vendor> Vendors { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
