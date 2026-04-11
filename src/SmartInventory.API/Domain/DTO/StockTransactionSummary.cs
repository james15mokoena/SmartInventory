namespace SmartInventory.API.Domain.DTO;


/// <summary>
/// A summary of the transaction of a specific product.
/// </summary>
public class StockTransactionSummary
{
    /// <summary>
    /// The name of the product/stock.
    /// </summary>
    public string ProductName { get; set; } = "";

    /// <summary>
    /// The start of the longest period without sales.
    /// </summary>
    public DateTime StartOfLongestPeriodWithoutSales { get; set; }

    /// <summary>
    /// The end of the longest period without sales.
    /// </summary>
    public DateTime EndOfLongestPeriodWithoutSales { get; set; }

    /// <summary>
    /// The start of the longest period with consecutive sales.
    /// </summary>
    public DateTime StartOfLongestPeriodWithConsecutiveSales { get; set; }

    /// <summary>
    /// The end of the longest period with consecutive sales.
    /// </summary>
    public DateTime EndOfLongestPeriodWithConsecutiveSales { get; set; }
    
    /// <summary>
    /// Stores the product's largest sale at any given day of the current month.
    /// </summary>
    public double LargestSale { get; set; }
    
    /// <summary>
    /// Stores the product's lowest sale at any given day of the current month.
    /// </summary>
    public double LowestSale { get; set; }
    
    /// <summary>
    /// Stores the product's total sales in the current month.
    /// </summary>
    public double TotalSales{ get; set; }
}