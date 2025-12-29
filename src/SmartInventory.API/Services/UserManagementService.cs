using SmartInventory.API.Domain.DTO;
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
    public bool CreateUser(Staff user)
    {
        if (IsDataValid(user) is Staff staff)
        {
            staff.PasswordHash = _passwordService.HashPassword(staff.PasswordHash);
            return _userManRepo.CreateUser(staff);
        }

        return false;
    }

    /// <summary>
    /// Checks if a user with the given username and password exists.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool CheckUserExistsByUsernameAndPassword(string username, string password) =>
                !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) &&
                _userManRepo.GetUserByUsername(username) is Staff staff &&
                _passwordService.VerifyPassword(password, staff.PasswordHash);
    
    /// <summary>
    /// Activates or deactivates a user (admin/staff).
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public bool ToggleUserActivation(string username) => _userManRepo.ToggleUserActivation(username);

    /// <summary>
    /// Gets a staff member with the given username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Staff? GetStaffMember(string username) => _userManRepo.GetStaffMember(username);

    /// <summary>
    /// Gets all active staff members.
    /// </summary>
    /// <returns></returns>
    public List<Staff>? GetActivatedStaff() => _userManRepo.GetActivatedStaff();

    /// <summary>
    /// Gets all deactivated staff members.
    /// </summary>
    /// <returns></returns>
    public List<Staff>? GetDeactivatedStaff() => _userManRepo.GetDeactivatedStaff();

    /// <summary>
    /// Edits a staff member's data.
    /// </summary>
    /// <param name="updatedStaffMember"></param>
    /// <returns></returns>
    public UserDto? EditStaffMember(UserDto updatedStaffMember)
    {
        if (updatedStaffMember.Id >= 0 && !string.IsNullOrEmpty(updatedStaffMember.FirstName) &&
           !string.IsNullOrEmpty(updatedStaffMember.LastName) && !string.IsNullOrEmpty(updatedStaffMember.Email) &&
           !string.IsNullOrEmpty(updatedStaffMember.Username) && updatedStaffMember.RoleId >= 0)
            return _userManRepo.EditStaffMember(updatedStaffMember) is Staff s ? updatedStaffMember : null;

        return null;
    }

    /// <summary>
    /// Checks if the user's data does not violate any contraints.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>the new user, otherwise null.</returns>
    private static IUser? IsDataValid(IUser user)
    {
        if (user is Staff newStaff)
            return (!string.IsNullOrEmpty(newStaff.Username) && !string.IsNullOrEmpty(newStaff.FirstName) &&
                   !string.IsNullOrEmpty(newStaff.LastName) && !string.IsNullOrEmpty(newStaff.Email) &&
                   !string.IsNullOrEmpty(newStaff.PasswordHash) && newStaff.IsActive &&
                   newStaff.DateCreated != default && newStaff.RoleId >= 0) ? newStaff : null;
        else if (user is Supplier newSupplier)
            return (!string.IsNullOrEmpty(newSupplier.ContactPersonEmail) && !string.IsNullOrEmpty(newSupplier.ContactPersonName) &&
                   !string.IsNullOrEmpty(newSupplier.ContactPersonPhone) && !string.IsNullOrEmpty(newSupplier.ContactPersonRole) &&
                   !string.IsNullOrEmpty(newSupplier.Address) && newSupplier.IsActive &&
                   newSupplier.DateCreated != default && !string.IsNullOrEmpty(newSupplier.Phone) &&
                   !string.IsNullOrEmpty(newSupplier.SupplierName) & !string.IsNullOrEmpty(newSupplier.Email)) ? newSupplier : null;

        return null;
    }

    /// <summary>
    /// Updates the active status of a Role options.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public bool ToggleRoleStatus(int roleId) => roleId >= 0 && _userManRepo.ToggleRoleStatus(roleId);

    /// <summary>
    /// Updates the active status of a Permission options.
    /// </summary>
    /// <param name="permId"></param>
    /// <returns></returns>
    public bool TogglePermissionStatus(int permId) => permId >= 0 && _userManRepo.TogglePermissionStatus(permId);

    /// <summary>
    /// Adds a new role.
    /// </summary>
    /// <param name="newRole"></param>
    /// <returns></returns>
    public bool AddRole(RoleDto newRole) => !string.IsNullOrEmpty(newRole.Name) && _userManRepo.AddRole(newRole);

    /// <summary>
    /// Adds a new permission.
    /// </summary>
    /// <param name="newPerm"></param>
    /// <returns></returns>
    public bool AddPermission(PermissionDto newPerm) => !string.IsNullOrEmpty(newPerm.Name) && _userManRepo.AddPermission(newPerm);
}