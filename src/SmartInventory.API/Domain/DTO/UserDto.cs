namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// Used to expose limited admin's or staff members's data.
/// </summary>
public class UserDto
{
    /// <summary>
    /// A unique identifier for the user.
    /// </summary>
    public int Id { get; set; } = -1;

    /// <summary>
    /// The first name of the user.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// The last name of the user.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// The email of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The user's chosen login username.
    /// </summary>    
    public string? Username { get; set; }

    /// <summary>
    /// Indicates the role of the user.
    /// </summary>
    public int RoleId { get; set; } = -1;
}