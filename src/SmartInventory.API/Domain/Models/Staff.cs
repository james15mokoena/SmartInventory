using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents a staff member.
/// </summary>
[Table("Staff")]
[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email),IsUnique =true)]
public class Staff : IUser
{

    /// <summary>
    /// A unique identifier for the staff.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }

    /// <summary>
    /// The first name of the staff.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// The last name of the staff.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// The email of the staff.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The staff's chosen login username.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// The staff's hashed password.
    /// </summary>
    public required string PasswordHash { get; set; }

    /// <summary>
    /// Indicates the role of the staff.
    /// </summary>
    public required int RoleId { get; set; }

    /// <summary>
    /// Indicates if the staff is still active in the company or is deactivated. <br/><br/>
    /// <strong>Note:</strong> In the Analysis document it is named <em>Status</em>.
    /// </summary>
    public required bool IsActive { get; set; }

    /// <summary>
    /// The date on which the staff was added to the system.
    /// </summary>
    public required DateTime DateCreated { get; set; }

    /// <summary>
    /// The date on which the staff last logged in.
    /// </summary>
    public DateTime LastLoginDate { get; set; }
}