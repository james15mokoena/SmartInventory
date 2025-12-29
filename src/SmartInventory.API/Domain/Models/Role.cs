using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents a user role.
/// </summary>
[Table("Role")]
public class Role
{
    /// <summary>
    /// An identifier for the role.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }

    /// <summary>
    /// The name of the role, for example, Admin, Staff, Senior Staff, etc.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Indicates if this role is still being used.
    /// </summary>
    public required bool IsActive { get; set; }

    // navigation properties

    /// <summary>
    /// Permissions assigned to this role.
    /// </summary>
    public List<Permission> Permissions { get; set; } = [];
}