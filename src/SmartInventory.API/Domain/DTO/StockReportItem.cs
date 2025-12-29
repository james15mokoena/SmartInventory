namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// An item to be shown on the stock report.
/// </summary>
public class StockReportItem
{
    /// <summary>
    /// The name of the item.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// A unique identifier for the item.
    /// </summary>
    public string? Code { get; set; }
    
    /// <summary>
    /// The number of items available currently (in units).
    /// </summary>
    public int StockLevel { get; set; }
    
    /// <summary>
    /// The threshold at which the item must be reordered (in units).
    /// </summary>
    public int ReorderLevel { get; set; }
    
    /// <summary>
    /// The maximum number of items that can be available (in units).
    /// </summary>
    public int MaximumLevel { get; set; }
    
    /// <summary>
    /// Classifies the item.
    /// </summary>
    public string? Category{ get; set; }
}