namespace SmartInventory.API.Domain.DTO;

/// <summary>
/// Represents a tax invoice issued upon sale/purchase.
/// </summary>
public class TaxInvoiceDto
{
    /// <summary>
    /// A unique identifier for the invoice.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The username of the person who generated the tax invoice.
    /// </summary>
    public string GeneratedBy { get; set; } = "";

    /// <summary>
    /// The account number.
    /// </summary>
    public string AccountNo { get; set; } = "";

    /// <summary>
    /// The date on which the invoice was generated.
    /// </summary>
    public DateTime DateGenerated { get; set; } = DateTime.Today;

    /// <summary>
    /// The house/building number and street name of the customer.
    /// </summary>
    public string CustomerHouseNoAndStreetName { get; set; } = "";

    /// <summary>
    /// The town/suburb and code of where the customer is located.
    /// </summary>
    public string CustomerTownAndCode { get; set; } = "";

    /// <summary>
    /// The customer's VAT registration number (if any).
    /// </summary>
    public string CustomerVatRegNo { get; set; } = "";

    /// <summary>
    /// The method of payment can be one of: Cheque, Cash, Credit Card or Account.
    /// </summary>
    public string MethodOfPayment { get; set; } = "";

    /// <summary>
    /// The builing number and street name of the company.
    /// </summary>
    public string CompanyBuldingNoAndStreetName { get; set; } = "";

    /// <summary>
    /// The company's town and code.
    /// </summary>
    public string CompanyTownAndCode { get; set; } = "";

    /// <summary>
    /// The company's telephone/cellphone.
    /// </summary>
    public string CompanyTelephoneNo { get; set; } = "";

    /// <summary>
    /// The company's Fax number.
    /// </summary>
    public string FaxNo { get; set; } = "";

    /// <summary>
    /// The post office box address of the company.
    /// </summary>
    public string CompanyPoBoxAddress { get; set; } = "";

    /// <summary>
    /// The town/suburb where the post office is located.
    /// </summary>
    public string CompanyPoBoxTownAndCode { get; set; } = "";

    /// <summary>
    /// The VAT registration number of the company (if any).
    /// </summary>
    public string VatRegNo { get; set; } = "";

    /// <summary>
    /// The name of the company.
    /// </summary>
    public string CompanyName { get; set; } = "";

    /// <summary>
    /// The name of the customer.
    /// </summary>
    public string CustomerName { get; set; } = "";

    /// <summary>
    /// The items sold.
    /// </summary>
    public List<TaxInvoiceItemDto> InvoiceItems { get; set; } = [];
}