using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents an administrator.
/// </summary>
[Table("Admin")]
[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email),IsUnique =true)]
public class Admin : IUser
{
    /// <summary>
    /// A unique identifier for the admin.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// The first name of the admin.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// The last name of the admin.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// The email of the admin.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The admin's chosen login username.
    /// </summary>    
    public required string Username { get; set; }

    /// <summary>
    /// The admin's hashed password.
    /// </summary>
    public required string PasswordHash { get; set; }

    /// <summary>
    /// Indicates the role of the admin.
    /// </summary>
    public required int RoleId { get; set; }

    /// <summary>
    /// Indicates if the admin is still active in the company or is deactivated. <br/><br/>
    /// <strong>Note:</strong> In the Analysis document it is named <em>Status</em>.
    /// </summary>
    public required bool IsActive { get; set; }

    /// <summary>
    /// The date on which the admin was added to the system.
    /// </summary>
    public required DateTime DateCreated { get; set; }

    /// <summary>
    /// The date on which the admin last logged in.
    /// </summary>
    public DateTime LastLoginDate { get; set; }
}