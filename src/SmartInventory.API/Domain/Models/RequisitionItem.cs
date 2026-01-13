using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents an item that is requested to be purchased on the Requisition form.
/// </summary>
[Table("RequisitionItem")]
public class RequisitionItem
{
    /// <summary>
    /// A unique ID for the item.
    /// </summary>
    [Key]
    public required string Code { get; set; }
    
    /// <summary>
    /// The name of the item or a description it.
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// The amount being requested to be purchased.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// The selling price of the item.
    /// </summary>
    public double SellingPrice { get; set; }

    /// <summary>
    /// The foreign key for the Requisition that contains this item.
    /// </summary>
    public int RequisitionId { get; set; }

    // Navigation property

    public required Requisition Requisition { get; set; }
}