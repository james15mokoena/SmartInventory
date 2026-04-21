namespace SmartInventory.API.Domain.DTO;

public class TotalSalesByCategory
{
    /// <summary>
    /// The product category.
    /// </summary>
    public string? Category { get; set; } = "";

    /// <summary>
    /// The name of the product. <em>This is used for queries about a product
    /// in the category that is most or least selling.</em>
    /// </summary>
    public string? ProductName { get; set; } = "";

    /// <summary>
    /// The total sales for the category.
    /// </summary>
    public double TotalSales { get; set; }
}