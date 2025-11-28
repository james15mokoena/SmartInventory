using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Models;

/// <summary>
/// Represents a transaction performed on stocks.
/// </summary>
[Table("StockTransaction")]
public class StockTransaction
{
    /// <summary>
    /// A unique identifier for each stock transaction.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int TransactionId { get; set; }

    /// <summary>
    /// Identifies which product the stock change applies to.
    /// </summary>
    public required string ProductId { get; set; }

    /// <summary>
    /// Identifies the admin or staff who performed the action.
    /// </summary>
    public required int UserId { get; set; }

    /// <summary>
    /// The date on which the transaction occurred.
    /// </summary>
    public required DateTime Date { get; set; }

    /// <summary>
    /// The amount by which the stock changed.
    /// </summary>
    public required int QuantityChange { get; set; }

    /// <summary>
    /// Provide clarity on the nature of the transaction.
    /// </summary>
    public required int ReasonTypeId { get; set; }

    /// <summary>
    /// The stock quantity before this transaction was applied.
    /// </summary>
    public required int PreviousStock { get; set; }

    /// <summary>
    /// The stock quantity after the transaction was applied.
    /// </summary>
    public required int NewStock { get; set; }

    // navigation properties

    /// <summary>
    /// A transaction may apply to one product.
    /// </summary>
    public required Product Product { get; set; }
}