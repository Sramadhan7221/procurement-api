using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Procurement.Domain.Entities;

namespace Procurement.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Role> Roles { get; }
    DbSet<Menu> Menus { get; }
    DbSet<RoleMenu> RoleMenus { get; }
    DbSet<User> Users { get; }
    DbSet<ProcurementRequest> ProcurementRequests { get; }
    DbSet<ProcurementItem> ProcurementItems { get; }
    DbSet<Invoice> Invoices { get; }
    DbSet<Category> Categories { get; }
    DbSet<Division> Divisions { get; }
    DbSet<Product> Products { get; }
    DbSet<Vendor> Vendors { get; }
    DbSet<PurchaseOrder> PurchaseOrders { get; }
    DbSet<GoodsReceipt> GoodsReceipts { get; }
    DbSet<GoodsReceiptItem> GoodsReceiptItems { get; }
    DbSet<ProcurementInvoice> ProcurementInvoices { get; }
    DbSet<ProcurementInvoiceItem> ProcurementInvoiceItems { get; }
    DbSet<ProcurementPayment> ProcurementPayments { get; }
    DbSet<ProcurementAuditTrail> ProcurementAuditTrails { get; }

    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
