namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// Represents a sold item.
/// </summary>
public class TaxInvoiceItemDto
{
    /// <summary>
    /// A unique identifier of the item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The product SKU.
    /// </summary>
    public string Code { get; set; } = "";

    /// <summary>
    /// The name/description of the product.
    /// </summary>
    public string Description { get; set; } = "";

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
}