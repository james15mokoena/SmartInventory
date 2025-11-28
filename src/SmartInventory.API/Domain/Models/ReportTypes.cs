namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Defines the types of reports that can be generated.
/// </summary>
public static class ReportTypes
{

    public const string StockValuation = "StockValuation";

    public const string Shrinkage = "Shrinkage";

    public const string Forecast = "Forecast";

    public const string FastMoving = "FastMoving";

    public const string MonthlyUsage = "MonthlyUsage";
}