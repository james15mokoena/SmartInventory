using SmartInventory.API.Data;

namespace SmartInventory.API.Repositories;

/// <summary>
/// Defines the functionality for providing services performed by the purchase
/// department.
/// </summary>
public class ProcurementManagementRepository(DatabaseContext context)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly DatabaseContext _context = context;


}