using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents an item on the order.
/// </summary>
[Table("OrderItem")]
public class OrderItem
{
    /// <summary>
    /// A unique ID of the ordered item.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The SKU of the item ordered.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// A description/name of the item ordered.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The number of items/units of ordered.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the item VAT inclusive.
    /// </summary>
    public double UnitPrice { get; set; }

    /// <summary>
    /// THe total price of the units ordered VAT inclusive.
    /// </summary>
    public double TotalAmount { get; set; }

    /// <summary>
    /// A reference to the order that contains this item.
    /// </summary>
    public int OrderNo { get; set; }

    /// <summary>
    /// The product being ordered.
    /// </summary>
    public required Product Product { get; set; }

    /// <summary>
    /// The order that contains this item.
    /// </summary>
    public required Order Order { get; set; }
}