namespace SmartInventory.API.Models;

/// <summary>
/// Indicates permissions that a role has.
/// </summary>
public class RolePermission
{
    /// <summary>
    /// Links with a role.
    /// </summary>
    public required int RoleId { get; set; }
    
    /// <summary>
    /// Links with a permission.
    /// </summary>
    public required int PermissionId { get; set; }

    // Navigation properties

    /// <summary>
    /// A role can have one or more permissions.
    /// </summary>
    public required Role Role { get; set; }

    /// <summary>
    /// A permission can be assigned to one or more roles.
    /// </summary>
    public required Permission Permission { get; set; }
}