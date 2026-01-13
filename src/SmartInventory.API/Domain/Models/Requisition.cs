using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents a generated requisition form.
/// </summary>
[Table("Requisition")]
public class Requisition
{
    /// <summary>
    /// A unique requisition number for the generated requisition form.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The name of the person who generated the requisition form.
    /// </summary>
    public required string AuthorisedBy { get; set; }

    /// <summary>
    /// The date on which the requisition form was generated.
    /// </summary>
    public required DateTime DateGenerated { get; set; }

    /// <summary>
    /// The name of the department that generated the form, e.g. Sales, Admin, etc.
    /// </summary>
    public required string FromDepartment { get; set; }

    // Navigation property

    public List<RequisitionItem> RequisitionItems { get; set; } = [];
}