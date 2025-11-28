using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Models;

/// <summary>
/// Represents a product.
/// </summary>
[Table("Product")]
public class Product
{
    /// <summary>
    /// A unique identifier for the product.<br/>
    /// SKU stands for "Stock Keeping Unit".
    /// </summary>
    [Key]
    public required string SKU { get; set; }

    /// <summary>
    /// The name of the product.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// A text describing the product.
    /// </summary>
    [MaxLength(255)]
    public required string Description { get; set; }

    /// <summary>
    /// Links the product to its default supplier.
    /// </summary>
    public int SupplierId { get; set; }

    /// <summary>
    /// The selling price of the product.
    /// </summary>
    public double UnitPrice { get; set; }

    /// <summary>
    /// The price the business pays the supplier for one unit.
    /// </summary>
    public double CostPrice { get; set; }

    /// <summary>
    /// The threshold below which the product becomes <strong>low stock</strong>.
    /// </summary>
    public int MinimumStockLevel { get; set; }

    /// <summary>
    /// The real time quantity available.
    /// </summary>
    public int CurrentStock { get; set; }

    /// <summary>
    /// The default quantity to order when stock is low.
    /// </summary>
    public int ReorderQuantity { get; set; }

    /// <summary>
    /// The group under which the product is classified.
    /// </summary>
    public required string Category { get; set; }

    /// <summary>
    /// The size of the product.
    /// </summary>
    public float UnitMeasurement { get; set; }

    /// <summary>
    /// Indicates if the product is available, to avoid deleting stock history.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// For audits and tracking when the product was added.
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Tracks when the product details were last modified.
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// A numeric or alphanumeric code printed on the product.
    /// </summary>
    public string Barcode { get; set; } = "";

    // Navigation properties.

    /// <summary>
    /// A product may be supplied by one <strong>Supplier</strong>.
    /// </summary>
    public required Supplier Supplier { get; set; }

    /// <summary>
    /// A product may have one or more stock transactions.
    /// </summary>
    public required List<StockTransaction> StockTransactions { get; set; } = [];

    /// <summary>
    /// A product may correspond to one or more purchase order items.
    /// </summary>
    public required List<PurchaseOrderItem> PurchaseOrderItems { get; set; } = [];
}