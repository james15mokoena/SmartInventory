using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents a generated quotation.
/// </summary>
[Table("Quotation")]
public class Quotation
{
    /// <summary>
    /// A unique ID for the quotation.
    /// </summary>
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// The date on which the quotation was generated.
    /// </summary>
    public required DateTime DateGenerated { get; set; }
    
    /// <summary>
    /// The name of the person who generated the quote.
    /// </summary>
    public required string QuotedBy { get; set; }
    
    /// <summary>
    /// The signature of the person who generated the quote or who authorized it.
    /// </summary>
    public required string Signature { get; set; }
    
    /// <summary>
    /// The person/business that requested the quotation.
    /// </summary>
    public required string SuppliedTo { get; set; }

    /// <summary>
    /// The items that the customer requests a quotation for.
    /// </summary>
    public List<QuotationItem> QuotationItems { get; set; } = [];
}