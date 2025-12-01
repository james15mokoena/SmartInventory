namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// Used to expose limited supplier data.
/// </summary>
public class SupplierDto
{
    /// <summary>
    /// A unique ID or number of a supplier.
    /// </summary>
    public int Id { get; set; } = -1;

    /// <summary>
    /// The name of the supplier.
    /// </summary>
    public string? SupplierName { get; set; }

    /// <summary>
    /// The name of the person they interact with.
    /// </summary>
    public string? ContactPersonName { get; set; }

    /// <summary>
    /// The email of the person they interact with.
    /// </summary>
    public string? ContactPersonEmail { get; set; }

    /// <summary>
    /// The phone number of the person they interact with.
    /// </summary>
    public string? ContactPersonPhone { get; set; }

    /// <summary>
    /// The role of the person they interact with.
    /// </summary>
    public string? ContactPersonRole { get; set; }

    /// <summary>
    /// The physical address of the supplier.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// The phone number of the supplier.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// The email of the supplier.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The URL of the supplier's website.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Indicates whether the supplier is still active or deactivated.
    /// </summary>
    public required bool IsActive { get; set; }

     /// <summary>
    /// The date on which the supplier was added to the system.
    /// </summary>
    public required DateTime DateCreated { get; set; }
}