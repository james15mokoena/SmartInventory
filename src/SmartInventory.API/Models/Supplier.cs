using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SmartInventory.API.Models;

/// <summary>
/// Represents a supplier.
/// </summary>
[Table("Supplier")]
[Index(nameof(ContactPersonEmail), IsUnique = true)]
[Index(nameof(ContactPersonPhone), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Phone), IsUnique = true)]
public class Supplier
{
    /// <summary>
    /// A unique idenfier for the supplier.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// The name of the supplier.
    /// </summary>
    public required string SupplierName { get; set; }

    /// <summary>
    /// The name of the person they interact with.
    /// </summary>
    public required string ContactPersonName { get; set; }

    /// <summary>
    /// The email of the person they interact with.
    /// </summary>
    public required string ContactPersonEmail { get; set; }

    /// <summary>
    /// The phone number of the person they interact with.
    /// </summary>
    public required string ContactPersonPhone { get; set; }

    /// <summary>
    /// The role of the person they interact with.
    /// </summary>
    public required string ContactPersonRole { get; set; }

    /// <summary>
    /// The physical address of the supplier.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// The phone number of the supplier.
    /// </summary>
    public required string Phone { get; set; }

    /// <summary>
    /// The email of the supplier.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The URL of the supplier's website.
    /// </summary>
    public string? Website { get; set; } = "";

    /// <summary>
    /// Indicates whether the supplier is still active or deactivated.
    /// </summary>
    public required string IsActive { get; set; }

    /// <summary>
    /// The date on which the supplier was added to the system.
    /// </summary>
    public required string DateCreated { get; set; }

    // Navigation properties

    /// <summary>
    /// A supplier may supplier one or more <strong>Product</strong>s.
    /// </summary>
    public List<Product> Products { get; set; } = [];

    /// <summary>
    /// A supplier may receive or more purchase orders.
    /// </summary>
    public List<PurchaseOrder> PurchaseOrders { get; set; } = [];
}