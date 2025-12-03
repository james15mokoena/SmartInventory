namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// Used to limit the StockTransaction data to be exposed.
/// </summary>
public class StockTransactionDto
{
    /// <summary>
    /// A unique identifier for each stock transaction.
    /// </summary>
    public int TransactionId { get; set; }

    /// <summary>
    /// Identifies which product the stock change applies to.
    /// </summary>
    public string? ProductId { get; set; }

    /// <summary>
    /// Identifies the admin or staff who performed the action.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// The date on which the transaction occurred.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// The amount by which the stock changed.
    /// </summary>
    public int QuantityChange { get; set; }

    /// <summary>
    /// Provide clarity on the nature of the transaction.
    /// </summary>
    public int ReasonTypeId { get; set; }

    /// <summary>
    /// The stock quantity before this transaction was applied.
    /// </summary>
    public int PreviousStock { get; set; }

    /// <summary>
    /// The stock quantity after the transaction was applied.
    /// </summary>
    public int NewStock { get; set; }
}