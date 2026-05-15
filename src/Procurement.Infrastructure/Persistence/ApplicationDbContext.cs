using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<RoleMenu> RoleMenus => Set<RoleMenu>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ProcurementRequest> ProcurementRequests => Set<ProcurementRequest>();
    public DbSet<ProcurementItem> ProcurementItems => Set<ProcurementItem>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Division> Divisions => Set<Division>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<GoodsReceipt> GoodsReceipts => Set<GoodsReceipt>();
    public DbSet<GoodsReceiptItem> GoodsReceiptItems => Set<GoodsReceiptItem>();
    public DbSet<ProcurementInvoice> ProcurementInvoices => Set<ProcurementInvoice>();
    public DbSet<ProcurementInvoiceItem> ProcurementInvoiceItems => Set<ProcurementInvoiceItem>();
    public DbSet<ProcurementPayment> ProcurementPayments => Set<ProcurementPayment>();
    public DbSet<ProcurementAuditTrail> ProcurementAuditTrails => Set<ProcurementAuditTrail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
