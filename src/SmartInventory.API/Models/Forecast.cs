namespace SmartInventory.API.Models;

/// <summary>
/// Will store generated forecast data.
/// </summary>
public class Forecast
{
    /// <summary>
    /// A unique identifier for the forecast.
    /// </summary>
    public int ForecastId { get; set; }
    
    /// <summary>
    /// The product the forecast is for.
    /// </summary>
    public int ProductId { get; set; }
    
    /// <summary>
    /// The date for which the prediction applies.
    /// </summary>
    public DateTime ForecastDate { get; set; }
    
    /// <summary>
    /// The number of units expected to be needed/sold/used on that date.
    /// </summary>
    public int ForecastedQuantity { get; set; }
    
    /// <summary>
    /// The date on when the forecast was computed.
    /// </summary>
    public DateTime GeneratedOn { get; set; }

    /// <summary>
    /// Indicates the forecasting algorithm used.
    /// </summary>
    public string MethodUsed { get; set; } = "";
}