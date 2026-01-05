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
    /// The stock level of items in the stock room when the report was made out.
    /// </summary>
    public int StockLevel { get; set; }
    
    /// <summary>
    /// The re-ordering level , this shows the level at which stock needs to be reordered.
    /// </summary>
    public int ReorderLevel { get; set; }
    
    /// <summary>
    /// The maximum level in units (this is the limit of units that should be held).
    /// </summary>
    public int MaximumLevel { get; set; }
    
    /// <summary>
    /// Classifies the item.
    /// </summary>
    public string? Category{ get; set; }
}