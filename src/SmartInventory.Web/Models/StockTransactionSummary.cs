namespace SmartInventory.Web.Models;

/// <summary>
/// A summary of the transaction of a specific product.
/// </summary>
public class StockTransactionSummary
{
    // SALES SUMMARY //

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
    public double TotalSales { get; set; }

    /// <summary>
    /// The number of units of the product sold this month.
    /// </summary>
    public int QuantityUnitsSold { get; set; }

    /// <summary>
    /// The number of units of the product unsold as of this month.
    /// </summary>
    public int QuantityUnitsUnsold { get; set; }

    // PURCHASES SUMMARY //

    /// <summary>
    /// Stores the number of times the product has been ordered this month.
    /// </summary>
    public int NumberOfTimesTheProductHasBeenOrdered { get; set; }

    /// <summary>
    /// The largest amount spent when purchasing this product this month.
    /// </summary>
    public double LargestOrder { get; set; }

    /// <summary>
    /// The lowest amount spent when purchasing this product this month.
    /// </summary>
    public double LowestOrder { get; set; }

    /// <summary>
    /// The total cost of the product this month.
    /// </summary>
    public double TotalCost { get; set; }

    /// <summary>
    /// The number of units of the product purchased as of this month?
    /// </summary>
    public int QuantityUnitsPurchased { get; set; }

    // RETURNED SUMMARY //

    /// <summary>
    /// The total amount reclaimed from units returned to the supplier.
    /// </summary>
    public double AmountReclaimed { get; set; }

    /// <summary>
    /// The number of units of this product returned to the supplier.
    /// </summary>
    public int QuantityReturned { get; set; }

    // DAMAGED SUMMARY //

    /// <summary>
    /// The number of units of the product that were damaged.
    /// </summary>
    public int QuantityDamaged { get; set; }

    /// <summary>
    /// The total cost of the damaged units of the product.
    /// </summary>
    public double DamageCost { get; set; }
}