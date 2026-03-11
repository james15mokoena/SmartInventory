namespace SmartInventory.Web.Models;

/// <summary>
/// Represents a reason for making a modification.
/// </summary>
public class TransactionReasonDto
{
    /// <summary>
    /// A unique identifier for the reason for making changes.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Indicates the reason for making modification.<br/>
    /// Possible reasons: Recevied, Issued, Adjusted, Damaged or Returned.
    /// </summary>
    public string? Reason { get; set; } = "";

    /// <summary>
    /// Indicates if this reason is still used, and avoids deleting it instead.
    /// </summary>
    public bool IsActive { get; set; } = true;
}