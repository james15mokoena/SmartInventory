namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// Provides information on the levels of stock/inventory.
/// </summary>
public class StockReport
{
    /// <summary>
    /// The name of the company.
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// The name of the source/support document.
    /// </summary>
    public string? DocName { get; set; } = "Stock (Inventory) Report";

    /// <summary>
    /// Identifies the person who generated the report.
    /// </summary>
    public string? Signature { get; set; }

    /// <summary>
    /// The date on which the report was generated.
    /// </summary>
    public DateTime DateGenerated { get; set; } = DateTime.Now;

    /// <summary>
    /// Contains all the stocks and their inventory levels.
    /// </summary>
    public List<StockReportItem> Items { get; set; } = [];
}