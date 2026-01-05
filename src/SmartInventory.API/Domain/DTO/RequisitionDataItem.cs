namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// Represents an item to be purchased on the requisition form.
/// </summary>
public class RequisitionDataItem
{
    /// <summary>
    /// A unique identifier for the item.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// A description of the item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The number of units to be purchased.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The price at which the item is sold.
    /// </summary>
    public double SellingPrice { get; set; }
}