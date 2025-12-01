namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// Used to expose limited admin data.
/// </summary>
public class AdminDto
{
    /// <summary>
    /// A unique identifier for the admin.
    /// </summary>
    public int Id { get; set; } = -1;

    /// <summary>
    /// The first name of the admin.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// The last name of the admin.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// The email of the admin.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The admin's chosen login username.
    /// </summary>    
    public string? Username { get; set; }

    /// <summary>
    /// The admin's hashed password.
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// Indicates the role of the admin.
    /// </summary>
    public int RoleId { get; set; } = -1;
}