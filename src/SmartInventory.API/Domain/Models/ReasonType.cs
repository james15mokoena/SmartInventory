using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents a reason for making a modification.
/// </summary>
[Table("ReasonType")]
public class ReasonType
{
    /// <summary>
    /// A unique identifier for the reason for making changes.
    /// </summary>
    [Key]
    public required int Id { get; set; }

    /// <summary>
    /// Indicates the reason for making modification.<br/>
    /// Possible reasons: Purchased, Sold, Damaged, Returned, New Product.
    /// </summary>
    public required string Reason { get; set; }

    /// <summary>
    /// Indicates if this reason is still used, and avoids deleting it instead.
    /// </summary>
    public required bool IsActive { get; set; }
}