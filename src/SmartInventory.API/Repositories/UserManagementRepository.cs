using SmartInventory.API.Data;
using SmartInventory.API.Domain.Models;

namespace SmartInventory.API.Repositories;

/// <summary>
/// Defines functionality for communicating with the database.
/// </summary>
public class UserManagementRepository(DatabaseContext context)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly DatabaseContext _context = context;

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="newUser"></param>
    /// <returns>true if user was created successfully, otherwise false.</returns>
    public bool CreateUser(IUser newUser)
    {
        if (newUser is Admin newAdmin)
            _context.Admins.Add(newAdmin);
        else if (newUser is Staff newStaff)
            _context.Staff.Add(newStaff);
        else if (newUser is Supplier newSupplier)
            _context.Suppliers.Add(newSupplier);
        
        return _context.SaveChanges() > 0;
    }    
}