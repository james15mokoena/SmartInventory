using SmartInventory.API.Domain.Models;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

/// <summary>
/// Defines the functionality that enforces the business rules/constraints.
/// </summary>
public class UserManagementService(UserManagementRepository userManagementRepository, PasswordService passwordService)
{
    /// <summary>
    /// Will be used to interact with the database.
    /// </summary>
    private readonly UserManagementRepository _userManRepo = userManagementRepository;

    private readonly PasswordService _passwordService = passwordService;

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="newUser"></param>
    /// <returns>true if user was created successfully, otherwise false.</returns>
    public bool CreateUser(IUser user)
    {
        if (IsDataValid(user) is Admin admin)
        {
            admin.PasswordHash = _passwordService.HashPassword(admin.PasswordHash);
            return _userManRepo.CreateUser(admin);
        }
        else if (IsDataValid(user) is Staff staff)
        {
            staff.PasswordHash = _passwordService.HashPassword(staff.PasswordHash);
            return _userManRepo.CreateUser(staff);
        }
        else if (IsDataValid(user) is Supplier supplier)
            return _userManRepo.CreateUser(supplier);

        return false;
    }
    
    /// <summary>
    /// Checks if a user with the given username and password exists.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool CheckUserExistsByUsernameAndPassword(string username, string password)
    {
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            if (_userManRepo.GetUserByUsernameAndPassword(username) is Admin admin)
                return _passwordService.VerifyPassword(password, admin.PasswordHash);
                
            if(_userManRepo.GetUserByUsernameAndPassword(username) is Staff staff)
                return _passwordService.VerifyPassword(password, staff.PasswordHash);
            
        }
            
        return false;
    }

    /// <summary>
    /// Checks if the user's data does not violate any contraints.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>the new user, otherwise null.</returns>
    private static IUser? IsDataValid(IUser user)
    {
        if (user is Admin newAdmin)
            return (!string.IsNullOrEmpty(newAdmin.Username) && !string.IsNullOrEmpty(newAdmin.FirstName) &&
                   !string.IsNullOrEmpty(newAdmin.LastName) && !string.IsNullOrEmpty(newAdmin.Email) &&
                   !string.IsNullOrEmpty(newAdmin.PasswordHash) && newAdmin.IsActive &&
                   newAdmin.DateCreated != default && newAdmin.RoleId >= 0) ? newAdmin : null;
        else if (user is Staff newStaff)
            return (!string.IsNullOrEmpty(newStaff.Username) && !string.IsNullOrEmpty(newStaff.FirstName) &&
                   !string.IsNullOrEmpty(newStaff.LastName) && !string.IsNullOrEmpty(newStaff.Email) &&
                   !string.IsNullOrEmpty(newStaff.PasswordHash) && newStaff.IsActive &&
                   newStaff.DateCreated != default && newStaff.RoleId >= 0) ? newStaff : null;
        else if (user is Supplier newSupplier)
            return (!string.IsNullOrEmpty(newSupplier.ContactPersonEmail) && !string.IsNullOrEmpty(newSupplier.ContactPersonName) &&
                   !string.IsNullOrEmpty(newSupplier.ContactPersonPhone) && !string.IsNullOrEmpty(newSupplier.ContactPersonRole) &&
                   !string.IsNullOrEmpty(newSupplier.Address) &&newSupplier.IsActive &&
                   newSupplier.DateCreated != default && !string.IsNullOrEmpty(newSupplier.Phone) &&
                   !string.IsNullOrEmpty(newSupplier.SupplierName) & !string.IsNullOrEmpty(newSupplier.Email)) ? newSupplier : null;

        return null;
    }
}