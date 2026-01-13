using SmartInventory.API.Data;

namespace SmartInventory.API.Services;

/// <summary>
/// Defines the functionality for providing services performed by the purchase
/// department.
/// </summary>
public class ProcurementManagementService(DatabaseContext context)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly DatabaseContext _context = context;


}