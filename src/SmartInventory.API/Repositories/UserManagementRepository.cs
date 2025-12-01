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

    /// <summary>
    /// Activates or deactivates a user (admin/staff).
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public bool ToggleUserActivation(string username)
    {
        if (!string.IsNullOrEmpty(username))
        {
            // check if its admin
            Admin? admin = _context.Admins.FirstOrDefault(a => a.Username == username);
            if (admin != null)
            {
                admin.IsActive = !admin.IsActive;
                _context.Update(admin);
                return _context.SaveChanges() > 0;
            }

            // check if its staff
            Staff? staff = _context.Staff.FirstOrDefault(s => s.Username == username);
            if (staff != null)
            {
                staff.IsActive = !staff.IsActive;
                _context.Update(staff);
                return _context.SaveChanges() > 0;
            }
        }

        return false;
    }

    /// <summary>
    /// Fetches an administrator with the given username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Admin? GetAdmin(string username) => _context.Admins.FirstOrDefault(a => a.Username == username) ?? null;

    /// <summary>
    /// Fetches a staff member with the given username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Staff? GetStaffMember(string username) => _context.Staff.FirstOrDefault(s => s.Username == username) ?? null;

    /// <summary>
    /// Fetches all activated administrators.
    /// </summary>
    /// <returns></returns>
    public List<Admin>? GetActivatedAdmins() => [.. _context.Admins.Where(a => a.IsActive == true)];

    /// <summary>
    /// Fetches all deactivated administrators.
    /// </summary>
    /// <returns></returns>
    public List<Admin>? GetDeactivatedAdmins() => [.. _context.Admins.Where(a => a.IsActive == false)];

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
    /// Edits an admin's data.
    /// </summary>
    /// <param name="updatedAdmin"></param>
    /// <returns></returns>
    public Admin? EditAdmin(UserDto updatedAdmin)
    {
        Admin? admin = GetAdmin(updatedAdmin.Username!);
        bool isUpdated = false;

        if (admin != null)
        {
            if (updatedAdmin.Username != admin.Username)
            {
                admin.Username = updatedAdmin.Username!;
                isUpdated = true;
            }

            if (updatedAdmin.Email != admin.Email)
            {
                admin.Email = updatedAdmin.Email!;
                isUpdated = true;
            }

            if (updatedAdmin.FirstName != admin.FirstName)
            {
                admin.FirstName = updatedAdmin.FirstName!;
                isUpdated = true;
            }

            if (updatedAdmin.LastName != admin.LastName)
            {
                admin.LastName = updatedAdmin.LastName!;
                isUpdated = true;
            }

            if (updatedAdmin.RoleId != admin.RoleId)
            {
                admin.RoleId = updatedAdmin.RoleId!;
                isUpdated = true;
            }

            if (isUpdated)
            {
                _context.Update(admin);
                return _context.SaveChanges() > 0 ? admin : null;
            }
        }

        return null;
    }
    
    /// <summary>
    /// Edits a staff member's data.
    /// </summary>
    /// <param name="updatedStaffMember"></param>
    /// <returns></returns>
    public Staff? EditStaffMember(UserDto updatedStaffMember)
    {
        Staff? staff = GetStaffMember(updatedStaffMember.Username!);
        bool isUpdated = false;

        if(staff != null)
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
}