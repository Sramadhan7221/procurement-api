using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ProcurementRequest> ProcurementRequests => Set<ProcurementRequest>();
    public DbSet<ProcurementItem> ProcurementItems => Set<ProcurementItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
