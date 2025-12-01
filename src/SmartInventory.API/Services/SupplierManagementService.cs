using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

/// <summary>
/// 
/// </summary>
/// <param name="suppRepo"></param>
public class SupplierManagementService(SupplierManagementRepository suppRepo)
{
    /// <summary>
    /// Enables interaction with the database.
    /// </summary>
    private readonly SupplierManagementRepository _suppManRepo = suppRepo;

    /// <summary>
    /// Adds a new supplier to the database.
    /// </summary>
    /// <param name="newSupplier"></param>
    /// <returns></returns>
    public bool CreateSupplier(SupplierDto newSupplier) //=> IsDataValid(newSupplier) && _suppManRepo.CreateSupplier(newSupplier);
    {
        if (newSupplier.Id >= 0 && !string.IsNullOrEmpty(newSupplier.Address) &&
           !string.IsNullOrEmpty(newSupplier.Email) && !string.IsNullOrEmpty(newSupplier.Phone) &&
           !string.IsNullOrEmpty(newSupplier.SupplierName) && !string.IsNullOrEmpty(newSupplier.Website) &&
           !string.IsNullOrEmpty(newSupplier.ContactPersonEmail) && !string.IsNullOrEmpty(newSupplier.ContactPersonName) &&
           !string.IsNullOrEmpty(newSupplier.ContactPersonPhone) && !string.IsNullOrEmpty(newSupplier.ContactPersonRole))
        {
            return _suppManRepo.CreateSupplier(new()
            {
                SupplierName = newSupplier.SupplierName,
                Email = newSupplier.Email,
                Address = newSupplier.Address,
                IsActive = newSupplier.IsActive,
                Phone = newSupplier.Phone,
                Website = newSupplier.Website,
                DateCreated = newSupplier.DateCreated,
                ContactPersonName = newSupplier.ContactPersonName,
                ContactPersonEmail = newSupplier.ContactPersonEmail,
                ContactPersonPhone = newSupplier.ContactPersonPhone,
                ContactPersonRole = newSupplier.ContactPersonRole,
                Id = newSupplier.Id,
                Products = [],
                PurchaseOrders = []
            });
        }

        return false;
    }

    /// <summary>
    /// Gets a supplier with the given supplier ID.
    /// </summary>
    /// <param name="supplierNo"></param>
    /// <returns></returns>
    public Supplier? GetSupplier(int supplierNo) => _suppManRepo.GetSupplier(supplierNo);

    /// <summary>
    /// Activates or deactivates supplier's account.
    /// </summary>
    /// <param name="supplierNo"></param>
    /// <returns></returns>
    public bool ToggleSupplierActiveStatus(int supplierNo) => _suppManRepo.ToggleSupplierActiveStatus(supplierNo);

    /// <summary>
    /// Gets all active suppliers.
    /// </summary>
    /// <returns></returns>
    public List<Supplier>? GetActiveSuppliers() => _suppManRepo.GetActiveSuppliers();

    /// <summary>
    /// Gets all deactivated suppliers.
    /// </summary>
    /// <returns></returns>
    public List<Supplier>? GetDeactivatedSuppliers() => _suppManRepo.GetDeactivatedSuppliers();

    /// <summary>
    /// Edits a supplier's data.
    /// </summary>
    /// <param name="updatedSupplier"></param>
    /// <returns></returns>
    public SupplierDto? EditSupplier(SupplierDto updatedSupplier)
    {
        if (updatedSupplier.Id >= 0 && !string.IsNullOrEmpty(updatedSupplier.Address) &&
           !string.IsNullOrEmpty(updatedSupplier.Email) && !string.IsNullOrEmpty(updatedSupplier.Phone) &&
           !string.IsNullOrEmpty(updatedSupplier.SupplierName) && !string.IsNullOrEmpty(updatedSupplier.Website) &&
           !string.IsNullOrEmpty(updatedSupplier.ContactPersonEmail) && !string.IsNullOrEmpty(updatedSupplier.ContactPersonName) &&
           !string.IsNullOrEmpty(updatedSupplier.ContactPersonPhone) && !string.IsNullOrEmpty(updatedSupplier.ContactPersonRole))
            return _suppManRepo.EditSupplier(updatedSupplier) is Supplier supplier? updatedSupplier : null;
        return null;
    }

    /// <summary>
    /// Checks if the supplier's data does not violate any constraints.
    /// </summary>
    /// <param name="supplier"></param>
    /// <returns></returns>
    private static bool IsDataValid(Supplier supplier)
    {
        return !string.IsNullOrEmpty(supplier.Address) && !string.IsNullOrEmpty(supplier.Email) &&
               !string.IsNullOrEmpty(supplier.SupplierName) && !string.IsNullOrEmpty(supplier.ContactPersonEmail) &&
               !string.IsNullOrEmpty(supplier.ContactPersonName) && !string.IsNullOrEmpty(supplier.ContactPersonPhone) &&
               !string.IsNullOrEmpty(supplier.ContactPersonRole) && !string.IsNullOrEmpty(supplier.Phone) &&
               supplier.DateCreated != default;
    }
}