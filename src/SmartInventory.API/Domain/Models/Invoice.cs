using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents a tax invoice issued upon sale/purchase.
/// </summary>
[Table("TaxInvoice")]
public class TaxInvoice
{
    /// <summary>
    /// A unique identifier for the invoice.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The account number.
    /// </summary>
    public string AccountNo { get; set; } = "";

    /// <summary>
    /// The date on which the invoice was generated.
    /// </summary>
    public required DateTime DateGenerated { get; set; }

    /// <summary>
    /// The house/building number and street name of the customer.
    /// </summary>
    public required string CustomerHouseNoAndStreetName { get; set; }

    /// <summary>
    /// The town/suburb and code of where the customer is located.
    /// </summary>
    public required string CustomerTownAndCode { get; set; }

    /// <summary>
    /// The customer's VAT registration number (if any).
    /// </summary>
    public string CustomerVatRegNo { get; set; } = "";

    /// <summary>
    /// The method of payment can be one of: Cheque, Cash, Credit Card or Account.
    /// </summary>
    public required string MethodOfPayment { get; set; }

    /// <summary>
    /// The builing number and street name of the company.
    /// </summary>
    public required string CompanyBuldingNoAndStreetName { get; set; }

    /// <summary>
    /// The company's town and code.
    /// </summary>
    public required string CompanyTownAndCode { get; set; }

    /// <summary>
    /// The company's telephone/cellphone.
    /// </summary>
    public required string CompanyTelephoneNo { get; set; }

    /// <summary>
    /// The company's Fax number.
    /// </summary>
    public string FaxNo { get; set; } = "";

    /// <summary>
    /// The post office box address of the company.
    /// </summary>
    public required string CompanyPoBoxAddress { get; set; }

    /// <summary>
    /// The town/suburb where the post office is located.
    /// </summary>
    public required string CompanyPoBoxTownAndCode { get; set; }

    /// <summary>
    /// The VAT registration number of the company (if any).
    /// </summary>
    public string VatRegNo { get; set; } = "";

    /// <summary>
    /// The items sold.
    /// </summary>
    public required List<InvoiceItem> InvoiceItems { get; set; }
}