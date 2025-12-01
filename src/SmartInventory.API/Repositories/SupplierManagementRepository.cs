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
}