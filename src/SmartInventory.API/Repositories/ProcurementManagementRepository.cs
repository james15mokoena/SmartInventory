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
}