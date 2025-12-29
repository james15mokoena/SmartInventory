using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents a permission.
/// </summary>
[Table("Permission")]
public class Permission
{
    /// <summary>
    /// A unique identifier for a permission.
    /// </summary>
    [Key]
    public required int Id { get; set; }

    /// <summary>
    /// The name of the permission, for example, CreateUser, AddProduct, etc.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// A text describing the permission.
    /// </summary>
    [MaxLength(255)]
    public required string Description { get; set; }

    /// <summary>
    /// Indicates if this permission is still being used.
    /// </summary>
    public required bool IsActive { get; set; }

    // navigation properties

    /// <summary>
    /// Roles that can be assigned permissions.
    /// </summary>
    public List<Role> Roles { get; set; } = [];
}