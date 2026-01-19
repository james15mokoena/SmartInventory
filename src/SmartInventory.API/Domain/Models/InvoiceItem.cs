using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents a sold item.
/// </summary>
[Table("InvoiceItem")]
public class InvoiceItem
{
    /// <summary>
    /// A unique identifier of the item.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The product SKU.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// The name/description of the product.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The number of units sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The price of a single item.
    /// </summary>
    public double UnitPrice { get; set; }

    /// <summary>
    /// The total price of the units sold.
    /// </summary>
    public double TotalPrice { get; set; }

    /// <summary>
    /// A reference to an invoice that contains this item.
    /// </summary>
    public int InvoiceId { get; set; }

    // navigation properties

    public required Product Product { get; set; }

    public required TaxInvoice Invoice { get; set; }
}