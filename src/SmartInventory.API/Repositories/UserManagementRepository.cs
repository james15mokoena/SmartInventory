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
    
    /// <summary>
    /// Verifies if there's a user with the given username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public IUser? GetUserByUsernameAndPassword(string username)
    {
        // check if its admin
        Admin? admin = _context.Admins.FirstOrDefault(a => a.Username == username);
        if (admin != null) return admin;

        // check if its staff
        Staff? staff = _context.Staff.FirstOrDefault(s => s.Username == username);
        if (staff != null) return staff;

        return null;
    }
}