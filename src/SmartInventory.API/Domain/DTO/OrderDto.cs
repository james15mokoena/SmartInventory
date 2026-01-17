namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// Represents a generated order to purchase some stocks.
/// </summary>
public class OrderDto
{
    /// <summary>
    /// A unique ID for the order.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the person who generated the order.
    /// </summary>
    public string? OrderedBy { get; set; } = "";

    /// <summary>
    /// The signature or name of the person who approved the order.
    /// </summary>
    public string? Signature { get; set; } = ""
;
    /// <summary>
    /// The name of the supplier.
    /// </summary>
    public string? SupplierName { get; set; } = "";

    /// <summary>
    /// The house number and the street name of the supplier.
    /// </summary>
    public string? SupplierHouseNoAndStreetName { get; set; } = "";

    /// <summary>
    /// The town/suburn/city and the code of where the supplier is located.
    /// </summary>
    public string? SupplierTownAndCode { get; set; } = "";

    /// <summary>
    /// The requisition number of the requisition form that resulted in the generation
    /// of this order.
    /// </summary>
    public int RequisitionNo { get; set; }

    /// <summary>
    /// The date of the requisition form that resulted in the generation of this
    /// order.
    /// </summary>
    public DateTime RequisitionDate { get; set; } = DateTime.Now;

    /// <summary>
    /// The date on which the order was generated.
    /// </summary>
    public DateTime DateGenerated { get; set; } = DateTime.Now;

    /// <summary>
    /// The number of the quotation that included the items/stocks in this order.
    /// </summary>
    public int QuotationNo { get; set; }

    /// <summary>
    /// The date on/before which the items ordered will be delivered to the customer/company.
    /// </summary>
    public DateTime DeliveryDate { get; set; } = DateTime.Now;

    /// <summary>
    /// The street number and street name of the company.
    /// </summary>
    public string? CompanyStreetNoAndStreetName { get; set; } = "";

    /// <summary>
    /// The post office box address of the company.
    /// </summary>
    public string? CompanyPoBoxAddress { get; set; } = "";

    /// <summary>
    /// The town/suburb where the post office is located.
    /// </summary>
    public string? CompanyPoBoxTownAndCode { get; set; } = "";

    /// <summary>
    /// The telephone/cellphone number of the company who generated the
    /// order.
    /// </summary>
    public string? CompanyTelephoneNo { get; set; } = "";

    /// <summary>
    /// The fax number of the company.
    /// </summary>
    public string? FaxNo { get; set; } = "";

    /// <summary>
    /// The VAT registration number of the customer (if any).
    /// </summary>
    public string? VatRegNo { get; set; } = "";

    /// <summary>
    /// The name of the company.
    /// </summary>
    public string? CompanyName { get; set; } = "";

    /// <summary>
    /// The items/stocks being ordered.
    /// </summary>
    public List<OrderItemDto> OrderItems { get; set; } = [];
}