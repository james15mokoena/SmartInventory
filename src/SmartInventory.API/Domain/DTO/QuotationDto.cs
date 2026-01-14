namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// Represents a generated quotation.
/// </summary>
public class QuotationDto
{
    /// <summary>
    /// A unique ID for the quotation.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The vat registration number of the business (if any).
    /// </summary>
    public string? VatRegNo { get; set; } = "";

    /// <summary>
    /// The date on which the quotation was generated.
    /// </summary>
    public DateTime DateGenerated { get; set; } = DateTime.Now;

    /// <summary>
    /// The name of the person who generated the quote.
    /// </summary>
    public string? QuotedBy { get; set; } = "";

    /// <summary>
    /// The signature of the person who generated the quote or who authorized it.
    /// </summary>
    public string? Signature { get; set; } = "";

    /// <summary>
    /// The person/business that requested the quotation.
    /// </summary>
    public string SuppliedTo { get; set; } = "";

    /// <summary>
    /// The name of the company.
    /// </summary>
    public string? Company { get; set; } = "";

    /// <summary>
    /// The company logo.
    /// </summary>
    public string? CompanyLogo { get; set; } = "";

    /// <summary>
    /// The building's number and street name where it is located in the area.
    /// </summary>
    public string? HouseNoAndStreetName { get; set; } = "";

    /// <summary>
    /// The name of the town or suburn where the company's building is located.
    /// </summary>
    public string? TownOrSuburb { get; set; } = "";

    /// <summary>
    /// The telephone number of the business.
    /// </summary>
    public string? TelephoneNo { get; set; } = "";

    /// <summary>
    /// The Post Office mailbox address.
    /// </summary>
    public string? PoBoxAddress { get; set; } = "";

    /// <summary>
    /// The area/suburb where the post office is located.
    /// </summary>
    public string? PostOfficeLocation { get; set; } = "";

    /// <summary>
    /// The Fax number.
    /// </summary>
    public string? FaxNo { get; set; } = "";

    /// <summary>
    /// The items that the customer requests a quotation for.
    /// </summary>
    public List<QuotationItemDto> QuotationItems { get; set; } = [];
}