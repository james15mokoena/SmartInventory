namespace SmartInventory.API.Domain.DTO;

public class PermissionDto
{
    /// <summary>
    /// A unique identifier for a permission.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the permission, for example, CreateUser, AddProduct, etc.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// A text describing the permission.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates if this permission is still being used.
    /// </summary>
    public bool IsActive { get; set; }
}