using SmartInventory.API.Data;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;

namespace SmartInventory.API.Repositories;

/// <summary>
/// 
/// </summary>
public class SupplierManagementRepository(DatabaseContext context)
{
    /// <summary>
    /// Used to send or retrieve data to/from the database.
    /// </summary>
    private readonly DatabaseContext _context = context;

    /// <summary>
    /// Adds a new supplier to the database.
    /// </summary>
    /// <param name="newSupplier"></param>
    /// <returns></returns>
    public bool CreateSupplier(Supplier newSupplier)
    {
        if (newSupplier != null)
        {
            _context.Suppliers.Add(newSupplier);
            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Fetches a supplier with the given ID.
    /// </summary>
    /// <param name="supplierNo"></param>
    /// <returns></returns>
    public Supplier? GetSupplier(int supplierNo) => _context.Suppliers.FirstOrDefault(s => s.Id == supplierNo);

    /// <summary>
    /// Activates or deactivates a supplier's account.
    /// </summary>
    /// <param name="supplierNo"></param>
    /// <returns></returns>
    public bool ToggleSupplierActiveStatus(int supplierNo)
    {
        Supplier? supplier = _context.Suppliers.FirstOrDefault(s => s.Id == supplierNo);
        if (supplier != null)
        {
            supplier.IsActive = !supplier.IsActive;
            _context.Update(supplier);
            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Fetches all active suppliers.
    /// </summary>
    /// <returns></returns>
    public List<Supplier>? GetActiveSuppliers() => [.. _context.Suppliers.Where(s => s.IsActive == true)];

    /// <summary>
    /// Fetches all deactivated suppliers.
    /// </summary>
    /// <returns></returns>
    public List<Supplier>? GetDeactivatedSuppliers() => [.. _context.Suppliers.Where(s => s.IsActive == false)];

    /// <summary>
    /// Edits a supplier's data.
    /// </summary>
    /// <param name="updatedSupplier"></param>
    /// <returns></returns>
    public Supplier? EditSupplier(SupplierDto updatedSupplier)
    {
        Supplier? supplier = GetSupplier(updatedSupplier.Id);
        bool isUpdated = false;

        if (supplier != null)
        {
            if (updatedSupplier.SupplierName != supplier.SupplierName)
            {
                supplier.SupplierName = updatedSupplier.SupplierName!;
                isUpdated = true;
            }

            if (updatedSupplier.Email != supplier.Email)
            {
                supplier.Email = updatedSupplier.Email!;
                isUpdated = true;
            }

            if (updatedSupplier.Address != supplier.Address)
            {
                supplier.Address = updatedSupplier.Address!;
                isUpdated = true;
            }

            if (updatedSupplier.Phone != supplier.Phone)
            {
                supplier.Phone = updatedSupplier.Phone!;
                isUpdated = true;
            }

            if (updatedSupplier.Website != supplier.Website)
            {
                supplier.Website = updatedSupplier.Website;
                isUpdated = true;
            }

            if (updatedSupplier.ContactPersonEmail != supplier.ContactPersonEmail)
            {
                supplier.ContactPersonEmail = updatedSupplier.ContactPersonEmail!;
                isUpdated = true;
            }

            if (updatedSupplier.ContactPersonName != supplier.ContactPersonName)
            {
                supplier.ContactPersonName = updatedSupplier.ContactPersonName!;
                isUpdated = true;
            }

            if (updatedSupplier.ContactPersonPhone != supplier.ContactPersonPhone)
            {
                supplier.ContactPersonPhone = updatedSupplier.ContactPersonPhone!;
                isUpdated = true;
            }

            if (updatedSupplier.ContactPersonRole != supplier.ContactPersonRole)
            {
                supplier.ContactPersonRole = updatedSupplier.ContactPersonRole!;
                isUpdated = true;
            }

            if (isUpdated)
            {
                _context.Update(supplier);
                if (_context.SaveChanges() > 0)
                    return supplier;
            }
        }

        return null;
    }
}