using SmartInventory.API.Data;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;

namespace SmartInventory.API.Repositories;

/// <summary>
/// Defines the functionality for the Purchase/Procurement department in the system.
/// </summary>
/// <param name="context"></param>
public class ProcurementManagementRepository(DatabaseContext context, ProductManagementRepository prodRepo)
{
    private readonly DatabaseContext _context = context;

    private readonly ProductManagementRepository _prodRepo = prodRepo;

    /// <summary>
    /// Stores a generated quotation in the database.
    /// </summary>
    /// <param name="quotation"></param>
    /// <returns></returns>
    public int AddQuotation(QuotationDto quotation)
    {
        if (!string.IsNullOrEmpty(quotation.QuotedBy) && !string.IsNullOrEmpty(quotation.SuppliedTo) && quotation.QuotationItems.Count > 0 &&
            quotation.DateGenerated != default)
        {
            Quotation quote = new()
            {
                Id = 0
                ,
                QuotedBy = quotation.QuotedBy
                ,
                Signature = quotation.Signature ?? ""
                ,
                SuppliedTo = quotation.SuppliedTo
                ,
                DateGenerated = quotation.DateGenerated
            };

            foreach (QuotationItemDto itemDto in quotation.QuotationItems)
            {
                // check if the item exists
                if (_prodRepo.GetProductBySku(itemDto.Code ?? "") is Product product && product.IsActive)
                {
                    quote.QuotationItems.Add(new()
                    {
                        Code = itemDto.Code!
                        ,
                        Description = itemDto.Description!
                        ,
                        Quantity = itemDto.Quantity
                        ,
                        QuotationId = quote.Id
                        ,
                        UnitPrice = itemDto.UnitPrice
                        ,
                        TotalPrice = itemDto.TotalPrice
                        ,
                        Quotation = quote
                    });
                }
            }

            if (quote.QuotationItems.Count > 0)
            {
                _context.Quotations.Add(quote);

                if (_context.SaveChanges() > 0)
                {
                    return _context.Quotations.FirstOrDefault(q => q.QuotedBy == quotation.QuotedBy && q.Signature == quotation.Signature &&
                     q.DateGenerated == quotation.DateGenerated && q.SuppliedTo == quotation.SuppliedTo)!.Id;
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Checks is a string is nor null or empty.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static bool IsStringValid(string? str) => !string.IsNullOrEmpty(str);

    /// <summary>
    /// Stores the order details.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>The order id.</returns>
    public int AddOrder(OrderDto dto)
    {
        if(IsStringValid(dto.CompanyName) && IsStringValid(dto.CompanyPoBoxAddress) && IsStringValid(dto.CompanyPoBoxTownAndCode) &&
            IsStringValid(dto.CompanyStreetNoAndStreetName) && IsStringValid(dto.CompanyTelephoneNo) && IsStringValid(dto.OrderedBy) &&
            IsStringValid(dto.Signature) && IsStringValid(dto.SupplierHouseNoAndStreetName) && IsStringValid(dto.SupplierName) &&
            IsStringValid(dto.SupplierTownAndCode) && dto.FaxNo != null && dto.VatRegNo != null && dto.DateGenerated != default &&
            dto.DeliveryDate != default && dto.RequisitionDate != default && dto.RequisitionNo >= 0 && dto.QuotationNo >= 0 &&
            dto.OrderItems.Count > 0)
        {
            Order order = new()
            {
                CompanyName = dto.CompanyName!
                ,
                CompanyPoBoxAddress = dto.CompanyPoBoxAddress!
                ,
                CompanyPoBoxTownAndCode = dto.CompanyPoBoxTownAndCode!
                ,
                CompanyStreetNoAndStreetName = dto.CompanyStreetNoAndStreetName!
                ,
                CompanyTelephoneNo = dto.CompanyTelephoneNo!
                ,
                DateGenerated = dto.DateGenerated
                ,
                DeliveryDate = dto.DeliveryDate
                ,
                RequisitionDate = dto.RequisitionDate
                ,
                FaxNo = dto.FaxNo
                ,
                OrderedBy = dto.OrderedBy!
                ,
                Signature = dto.Signature!
                ,
                SupplierName = dto.SupplierName!
                ,
                SupplierHouseNoAndStreetName = dto.SupplierHouseNoAndStreetName!
                ,
                SupplierTownAndCode = dto.SupplierTownAndCode!
                ,
                VatRegNo = dto.VatRegNo
                ,
                RequisitionNo = dto.RequisitionNo
                ,
                QuotationNo = dto.QuotationNo
                ,
                OrderItems = []
                ,
                Id = 0
            };

            foreach (OrderItemDto item in dto.OrderItems)
            {
                // check if item/product exists
                if (_prodRepo.GetProductBySku(item.Code!) is Product product)
                {
                    order.OrderItems.Add(new()
                    {
                        Code = product.SKU
                        ,
                        Description = product.Name
                        ,
                        Quantity = item.Quantity
                        ,
                        UnitPrice = item.UnitPrice
                        ,
                        TotalAmount = item.TotalAmount
                        ,
                        OrderNo = order.Id
                        ,
                        Order = order
                        ,
                        Product = product
                        ,
                        Id = 0
                    });
                }
            }
            
            // make sure the order has at least one item, before recording it.
            if(order.OrderItems.Count > 0)
            {
                _context.Orders.Add(order);
                if (_context.SaveChanges() > 0)
                {
                    return _context.Orders.FirstOrDefault(o => o.Signature == order.Signature && o.OrderedBy == order.OrderedBy &&
                        o.SupplierName == order.SupplierName && o.DateGenerated == order.DateGenerated && o.RequisitionDate ==
                        order.RequisitionDate && o.QuotationNo == order.QuotationNo && o.RequisitionNo == order.RequisitionNo &&
                        o.DeliveryDate == order.DeliveryDate)!.Id;
                }
            }
        }

        return -1;
    }
}