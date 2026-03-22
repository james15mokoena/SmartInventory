namespace SmartInventory.Web.Models;

/// <summary>
/// Provides information on the levels of stock/inventory.
/// </summary>
public class StockReportDto
{
    /// <summary>
    /// The name of the business.
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// The name of the source/support document.
    /// </summary>
    public string? DocName { get; set; } = "Stock (Inventory) Report";

    /// <summary>
    /// The name of the person responsible for managing stock.
    /// </summary>
    public string? Signature { get; set; }

    /// <summary>
    /// The date on which the report was generated.
    /// </summary>
    public DateTime DateGenerated { get; set; } = DateTime.Now;

    /// <summary>
    /// Contains all the stocks and their inventory levels.
    /// </summary>
    public List<StockReportItemDto> Items { get; set; } = [];
}