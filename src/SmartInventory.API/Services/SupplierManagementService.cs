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
    public bool CreateSupplier(Supplier newSupplier) => IsDataValid(newSupplier) && _suppManRepo.CreateSupplier(newSupplier);

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