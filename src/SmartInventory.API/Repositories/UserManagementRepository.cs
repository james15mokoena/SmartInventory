using SmartInventory.API.Data;
using SmartInventory.API.Domain.DTO;
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
    public bool CreateUser(Staff newStaff)
    {
        _context.Staff.Add(newStaff);
        
        return _context.SaveChanges() > 0;
    }

    /// <summary>
    /// Used to fetch a user by their username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Staff? GetUserByUsername(string username) => _context.Staff.FirstOrDefault(s => s.Username == username) ?? null;

    /// <summary>
    /// Activates or deactivates a user.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public bool ToggleUserActivation(string username)
    {
        if (!string.IsNullOrEmpty(username) && _context.Staff.FirstOrDefault(s => s.Username == username) is Staff staff)
        {
            staff.IsActive = !staff.IsActive;
            _context.Update(staff);
            return _context.SaveChanges() > 0;
        }
        
        return false;
    }

    /// <summary>
    /// Fetches a staff member with the given username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Staff? GetStaffMember(string username) => _context.Staff.FirstOrDefault(s => s.Username == username) ?? null;

    /// <summary>
    /// Fetches all activated staff members.
    /// </summary>
    /// <returns></returns>
    public List<Staff>? GetActivatedStaff() => [.. _context.Staff.Where(a => a.IsActive == true)];

    /// <summary>
    /// Fetches all deactivated staff members.
    /// </summary>
    /// <returns></returns>
    public List<Staff>? GetDeactivatedStaff() => [.. _context.Staff.Where(a => a.IsActive == false)];

    /// <summary>
    /// Edits a staff member's data.
    /// </summary>
    /// <param name="updatedStaffMember"></param>
    /// <returns></returns>
    public Staff? EditStaffMember(UserDto updatedStaffMember)
    {
        Staff? staff = GetStaffMember(updatedStaffMember.Username!);
        bool isUpdated = false;

        if (staff != null)
        {
            if (updatedStaffMember.Username != staff.Username)
            {
                staff.Username = updatedStaffMember.Username!;
                isUpdated = true;
            }

            if (updatedStaffMember.Email != staff.Email)
            {
                staff.Email = updatedStaffMember.Email!;
                isUpdated = true;
            }

            if (updatedStaffMember.FirstName != staff.FirstName)
            {
                staff.FirstName = updatedStaffMember.FirstName!;
                isUpdated = true;
            }

            if (updatedStaffMember.LastName != staff.LastName)
            {
                staff.LastName = updatedStaffMember.LastName!;
                isUpdated = true;
            }

            if (updatedStaffMember.RoleId != staff.RoleId)
            {
                staff.RoleId = updatedStaffMember.RoleId!;
                isUpdated = true;
            }

            if (isUpdated)
            {
                _context.Update(staff);
                return _context.SaveChanges() > 0 ? staff : null;
            }
        }

        return null;
    }

    /// <summary>
    /// Updates the active status of a Role.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public bool ToggleRoleStatus(int roleId)
    {
        if (_context.Roles.FirstOrDefault(r => r.Id == roleId) is Role role)
        {
            role.IsActive = !role.IsActive;
            _context.Update(role);
            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Updates the active status of a Permission.
    /// </summary>
    /// <param name="permId"></param>
    /// <returns></returns>
    public bool TogglePermissionStatus(int permId)
    {
        if (_context.Permissions.FirstOrDefault(p => p.Id == permId) is Permission permission)
        {
            permission.IsActive = !permission.IsActive;
            _context.Update(permission);
            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Adds a new role.
    /// </summary>
    /// <param name="newRole"></param>
    /// <returns></returns>
    public bool AddRole(RoleDto newRole)
    {
        if (!string.IsNullOrEmpty(newRole.Name) && _context.Roles.FirstOrDefault(r => r.Name == newRole.Name) == null)
        {
            _context.Roles.Add(new()
            {
                Id = 0
                ,
                Name = newRole.Name
                ,
                IsActive = true
                , Permissions = []
            });

            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Adds a new permission.
    /// </summary>
    /// <param name="newPerm"></param>
    /// <returns></returns>
    public bool AddPermission(PermissionDto newPerm)
    {
        if (!string.IsNullOrEmpty(newPerm.Name) && _context.Permissions.FirstOrDefault(p => p.Name == newPerm.Name) == null)
        {
            _context.Permissions.Add(new()
            {
                Id = 0
                ,
                Name = newPerm.Name
                ,
                IsActive = true
                ,
                Description = newPerm.Description!
                ,
                Roles = []
            });

            return _context.SaveChanges() > 0;
        }
        return false;
    }
    
    /// <summary>
    /// Assigns a permission 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="permission"></param>
    /// <returns></returns>
    public bool AssignPermission(string username, string permission)
    {
        return false;
    }
}