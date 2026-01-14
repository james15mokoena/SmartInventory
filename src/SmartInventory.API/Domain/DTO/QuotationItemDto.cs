namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// An item on a quotation.
/// </summary>
public class QuotationItemDto
{
    /// <summary>
    /// A unique ID for the item.
    /// </summary>
    public string? Code { get; set; } = "";

    /// <summary>
    /// A description or name of the item.
    /// </summary>
    public string? Description { get; set; } = "";

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
    public int QuotationId { get; set; }
}