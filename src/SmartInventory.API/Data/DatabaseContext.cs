using Microsoft.EntityFrameworkCore;
using SmartInventory.API.Domain.Models;

namespace SmartInventory.API.Data;

/// <summary>
/// Maps domain models to database tables.
/// </summary>
public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Admin> Admins { get; set; }
    
    public DbSet<Staff> Staff { get; set; }
    
    public DbSet<Supplier> Suppliers{ get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Role> Roles { get; set; }
    
    public DbSet<Permission> Permissions { get; set; }
    
    public DbSet<ReasonType> ReasonTypes { get; set; }

    public DbSet<RolePermission> RolePermissions { get; set; }
    
    public DbSet<StockTransaction> StockTransactions { get; set; }
    
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    
    public DbSet<PurchaseOrderItem> PurchaseOrderItems{ get; set; }

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
    }
}