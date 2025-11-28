using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Models;

/// <summary>
/// Represents a purchase order item.
/// </summary>
public class PurchaseOrderItem
{
    /// <summary>
    /// A uniqe identifier for a purchase order line item.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int PurchaseOrderItemId { get; set; }
    
    /// <summary>
    /// Identifies which product this purchase order item maps to.
    /// </summary>
    public required string ProductId { get; set; }

    /// <summary>
    /// Quantity requested from the supplier.
    /// </summary>
    public required int QuantityOrdered { get; set; }

    /// <summary>
    /// Cost of a single unit at the time the PO was created.
    /// </summary>
    public required double UnitCost { get; set; }

    /// <summary>
    /// StoredValue = QuantityOrdered * UnitCost.
    /// </summary>
    public required double TotalCost { get; set; }

    /// <summary>
    /// Tracks how much of this item has actually been delivered by the supplier.
    /// </summary>
    public required int QuantityReceived { get; set; }

    /// <summary>
    /// The date on which the item was fully received.
    /// </summary>
    public DateTime ReceivedDate { get; set; }

    /// <summary>
    /// Links the item to the purchase order.
    /// </summary>
    public required int PurchaseOrderId { get; set; }

    // navigation props

    /// <summary>
    /// A product may correspond to one purchase order line item.
    /// </summary>
    public required Product Product { get; set; }
    
    /// <summary>
    /// A purchase order may have one or more purchase order items.
    /// </summary>
    public required PurchaseOrder PurchaseOrder{ get; set; }
}