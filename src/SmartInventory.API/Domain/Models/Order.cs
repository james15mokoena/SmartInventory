using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Represents a generated order to purchase some stocks.
/// </summary>
[Table("Order")]
public class Order
{
    /// <summary>
    /// A unique ID for the order.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The name of the person who generated the order.
    /// </summary>
    public required string OrderedBy { get; set; }

    /// <summary>
    /// The signature or name of the person who approved the order.
    /// </summary>
    public required string Signature { get; set; }

    /// <summary>
    /// The name of the supplier.
    /// </summary>
    public required string SupplierName { get; set; }

    /// <summary>
    /// The house number and the street name of the supplier.
    /// </summary>
    public required string SupplierHouseNoAndStreetName { get; set; }

    /// <summary>
    /// The town/suburn/city and the code of where the supplier is located.
    /// </summary>
    public required string SupplierTownAndCode { get; set; }

    /// <summary>
    /// The requisition number of the requisition form that resulted in the generation
    /// of this order.
    /// </summary>
    public required int RequisitionNo { get; set; }

    /// <summary>
    /// The date of the requisition form that resulted in the generation of this
    /// order.
    /// </summary>
    public required DateTime RequisitionDate { get; set; }

    /// <summary>
    /// The date on which the order was generated.
    /// </summary>
    public required DateTime DateGenerated { get; set; }

    /// <summary>
    /// The number of the quotation that included the items/stocks in this order.
    /// </summary>
    public required int QuotationNo { get; set; }

    /// <summary>
    /// The date on/before which the items ordered will be delivered to the customer/company.
    /// </summary>
    public required DateTime DeliveryDate { get; set; }

    /// <summary>
    /// The street number and street name of the company.
    /// </summary>
    public required string CompanyStreetNoAndStreetName { get; set; }

    /// <summary>
    /// The post office box address of the company.
    /// </summary>
    public required string CompanyPoBoxAddress { get; set; }

    /// <summary>
    /// The town/suburb where the post office is located.
    /// </summary>
    public required string CompanyPoBoxTownAndCode { get; set; }

    /// <summary>
    /// The telephone/cellphone number of the company who generated the
    /// order.
    /// </summary>
    public required string CompanyTelephoneNo { get; set; }

    /// <summary>
    /// The fax number of the company.
    /// </summary>
    public string FaxNo { get; set; } = "";

    /// <summary>
    /// The VAT registration number of the customer (if any).
    /// </summary>
    public string VatRegNo { get; set; } = "";

    /// <summary>
    /// The name of the company.
    /// </summary>
    public required string CompanyName { get; set; }

    /// <summary>
    /// The items/stocks being ordered.
    /// </summary>
    public required List<OrderItem> OrderItems { get; set; } = [];
}