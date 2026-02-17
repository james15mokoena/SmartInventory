namespace SmartInventory.Web.Models;

public class RoleDto
{
    /// <summary>
    /// An identifier for the role.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the role, for example, Admin, Staff, Senior Staff, etc.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Indicates if this role is still being used.
    /// </summary>
    public bool IsActive { get; set; }
}