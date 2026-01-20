using SmartInventory.API.Data;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;

namespace SmartInventory.API.Repositories;

public class SalesManagementRepository(DatabaseContext context, ProductManagementRepository prodRepo)
{
    private readonly DatabaseContext _context = context;

    private readonly ProductManagementRepository _prodRepo = prodRepo;

    /// <summary>
    /// Stores a requisition indicating that a requisition request was generated.
    /// </summary>
    /// <param name="formData"></param>
    /// <returns></returns>
    public int AddRequisition(RequisitionFormData formData)
    {
        if (!string.IsNullOrEmpty(formData.Authorized) && !string.IsNullOrEmpty(formData.FromDepartment) &&
            formData.RequisitionDataItems.Count > 0)
        {
            Requisition requisition = new()
            {
                Id = 0
                ,
                AuthorisedBy = formData.Authorized!
                ,
                DateGenerated = formData.DateGenerated
                ,
                FromDepartment = formData.FromDepartment!
                ,
                RequisitionItems = []
            };

            foreach (RequisitionDataItem dataItem in formData.RequisitionDataItems)
            {
                // check if the product exists
                if (_prodRepo.GetProductBySku(dataItem.Code!) is Product product && product.IsActive)
                {
                    requisition.RequisitionItems.Add(new()
                    {
                        Code = dataItem.Code!
                        ,
                        Description = dataItem.Description!
                        ,
                        Quantity = dataItem.Quantity
                        ,
                        RequisitionId = requisition.Id
                        ,
                        SellingPrice = dataItem.SellingPrice
                        ,
                        Requisition = requisition
                    });
                }
            }

            if (requisition.RequisitionItems.Count > 0)
            {
                _context.Add(requisition);

                if (_context.SaveChanges() > 0)
                {
                    return _context.Requisitions.FirstOrDefault(r => r.AuthorisedBy == formData.Authorized && r.DateGenerated == formData.DateGenerated && r.FromDepartment == formData.FromDepartment)!.Id;
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Records a tax invoice.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public int AddTaxInvoice(TaxInvoiceDto dto)
    {
        if (IsStringValid(dto.AccountNo) && IsStringValid(dto.CompanyBuldingNoAndStreetName) && IsStringValid(dto.CompanyPoBoxAddress) &&
            IsStringValid(dto.CompanyPoBoxTownAndCode) && IsStringValid(dto.CompanyTelephoneNo) && IsStringValid(dto.CompanyTownAndCode) &&
            IsStringValid(dto.CustomerHouseNoAndStreetName) && IsStringValid(dto.CustomerTownAndCode) && dto.CustomerVatRegNo != null &&
            IsStringValid(dto.MethodOfPayment) && dto.VatRegNo != null && dto.DateGenerated != default && dto.FaxNo != null &&
            dto.InvoiceItems.Count > 0)
        {
            TaxInvoice invoice = new()
            {
                CompanyName = dto.CompanyName
                ,
                AccountNo = dto.AccountNo
                ,
                CompanyBuldingNoAndStreetName = dto.CompanyBuldingNoAndStreetName
                ,
                CompanyPoBoxAddress = dto.CompanyPoBoxAddress
                ,
                CompanyPoBoxTownAndCode = dto.CompanyPoBoxTownAndCode
                ,
                CompanyTelephoneNo = dto.CompanyTelephoneNo
                ,
                CompanyTownAndCode = dto.CompanyTownAndCode
                ,
                CustomerHouseNoAndStreetName = dto.CustomerHouseNoAndStreetName
                ,
                CustomerTownAndCode = dto.CustomerTownAndCode
                ,
                CustomerVatRegNo = dto.CustomerVatRegNo
                ,
                DateGenerated = dto.DateGenerated
                ,
                FaxNo = dto.FaxNo
                ,
                MethodOfPayment = dto.MethodOfPayment
                ,
                VatRegNo = dto.VatRegNo
                ,
                Id = 0
                ,
                CustomerName = dto.CustomerName
                ,
                InvoiceItems = []
            };            

            foreach (TaxInvoiceItemDto item in dto.InvoiceItems)
            {
                // check if the item is a product and is available.
                if (_prodRepo.GetProductBySku(item.Code) is Product product && product.IsActive)
                {
                    InvoiceItem invoiceItem = new()
                    {
                        Code = product.SKU
                        ,
                        Description = product.Name
                        ,
                        Id = 0
                        ,
                        Quantity = item.Quantity
                        ,
                        UnitPrice = item.UnitPrice
                        ,
                        TotalPrice = item.TotalPrice == (item.Quantity * item.UnitPrice) ? item.TotalPrice : (item.Quantity * item.UnitPrice)
                        ,
                        InvoiceId = invoice.Id
                        ,
                        Invoice = invoice
                        ,
                        Product = product
                    };

                    item.TotalPrice = invoiceItem.TotalPrice;
                    
                    invoice.InvoiceItems.Add(invoiceItem);
                }
            }

            if (invoice.InvoiceItems.Count > 0)
            {
                _context.Invoices.Add(invoice);
                if (_context.SaveChanges() > 0)
                    return _context.Invoices.FirstOrDefault(i => i.DateGenerated == invoice.DateGenerated && i.AccountNo == invoice.AccountNo &&
                        i.CustomerHouseNoAndStreetName == invoice.CustomerHouseNoAndStreetName && i.CompanyTelephoneNo == invoice.CompanyTelephoneNo
                        && i.MethodOfPayment == invoice.MethodOfPayment && i.CustomerTownAndCode == invoice.CustomerTownAndCode)!.Id;
            }
        }
        
        return -1;
    }

    /// <summary>
    /// Checks if a string is not null or empty.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static bool IsStringValid(string str) => !string.IsNullOrEmpty(str);
}