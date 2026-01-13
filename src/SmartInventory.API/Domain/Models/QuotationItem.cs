using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// An item on a quotation.
/// </summary>
[Table("QuotationItem")]
public class QuotationItem
{
    /// <summary>
    /// A unique ID for the item.
    /// </summary>
    [Key]
    public required string Code { get; set; }
    
    /// <summary>
    /// A description or name of the item.
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// The number of units requested.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// The price of a single item.
    /// </summary>
    public double UnitPrice { get; set; }
    
    /// <summary>
    /// The total price of the requested units.
    /// </summary>
    public double TotalPrice { get; set; }
    
    /// <summary>
    /// A foreign key of the quotation on which this item appears.
    /// </summary>
    public int QuotationId{ get; set; }

    // Navigation property

    public required Quotation Quotation{ get; set; }
}