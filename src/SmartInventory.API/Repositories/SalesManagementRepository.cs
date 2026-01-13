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
                if(_prodRepo.GetProductBySku(dataItem.Code!) is Product product && product.IsActive)
                {
                    requisition.RequisitionItems.Add(new()
                    {
                        Code = dataItem.Code!
                        ,
                        Description = dataItem.Description!
                        ,
                        Quantity = dataItem.Quantity
                        ,
                        RequisitionId = 0
                        ,
                        SellingPrice = dataItem.SellingPrice
                        ,
                        Requisition = requisition
                    });
                }
            }

            if(requisition.RequisitionItems.Count > 0)
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
}