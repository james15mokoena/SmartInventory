using SmartInventory.API.Data;
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
}