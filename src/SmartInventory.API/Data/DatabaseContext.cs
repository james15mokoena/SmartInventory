using Microsoft.EntityFrameworkCore;
using SmartInventory.API.Domain.Models;

namespace SmartInventory.API.Data;

/// <summary>
/// Maps domain models to database tables.
/// </summary>
public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    
    public DbSet<Staff> Staff { get; set; }
    
    public DbSet<Supplier> Suppliers{ get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Role> Roles { get; set; }
    
    public DbSet<Permission> Permissions { get; set; }
    
    public DbSet<ReasonType> ReasonTypes { get; set; }

    public DbSet<RolePermission> RolePermissions { get; set; }
    
    public DbSet<StockTransaction> StockTransactions { get; set; }
    
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    
    public DbSet<Requisition> Requisitions { get; set; }
    
    public DbSet<RequisitionItem> RequisitionItems { get; set; }
    
    public DbSet<Quotation> Quotations { get; set; }
    
    public DbSet<QuotationItem> QuotationItems { get; set; }
    
    public DbSet<Orders> Orders { get; set; }
    
    public DbSet<OrderItem> OrderItems{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // setting the foreign key for the Product.
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId)
            .IsRequired();

        // setting the foreign keys for the RolePermission.
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermission>();

        // setting the foreign key for the StockTransaction.
        modelBuilder.Entity<StockTransaction>()
            .HasOne(s => s.Product)
            .WithMany(p => p.StockTransactions)
            .HasForeignKey(s => s.ProductId);

        // setting the foreign key for the PurchaseOrderItem, with the PurchaseOrder.
        modelBuilder.Entity<PurchaseOrderItem>()
            .HasOne(i => i.PurchaseOrder)
            .WithMany(p => p.PurchaseOrderItems)
            .HasForeignKey(i => i.PurchaseOrderId);

        // setting the foreign key for the PurchaseOrderItem, with the Product.
        modelBuilder.Entity<PurchaseOrderItem>()
            .HasOne(i => i.Product)
            .WithMany(p => p.PurchaseOrderItems)
            .HasForeignKey(i => i.ProductId);

        // setting the foreign key for the RequisitionItem, with the Requisition.
        modelBuilder.Entity<RequisitionItem>()
            .HasOne(i => i.Requisition)
            .WithMany(r => r.RequisitionItems)
            .HasForeignKey(ri => ri.RequisitionId);

        // setting the foreign key for the QuotationItem, with the Quotation.
        modelBuilder.Entity<QuotationItem>()
            .HasOne(i => i.Quotation)
            .WithMany(r => r.QuotationItems)
            .HasForeignKey(ri => ri.QuotationId);

        // set the foreign key for the OrderItem with the Order.
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderNo);

        // set the foreign key for the OrderItem with the Product.
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.Code);
    }
}