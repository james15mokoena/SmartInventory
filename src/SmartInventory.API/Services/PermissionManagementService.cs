using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

/// <summary>
/// Purpose: Defines the functionality for creating, viewing and updating permissions and roles in the system
/// including assigning permissions to roles.
/// </summary>
public class PermissionManagementService(PermissionManagementRepository permMan)
{
    private readonly PermissionManagementRepository _permRepo = permMan;

    /// <summary>
    /// Purpose: Fetches all active permissions.
    /// </summary>
    /// <returns></returns>
    public List<PermissionDto>? GetActivePermissions() => _permRepo.GetActivePermissions();

    /// <summary>
    /// Purpose: Fetches all inactive permissions.
    /// </summary>
    /// <returns></returns>
    public List<PermissionDto>? GetDeactivatedPermissions() => _permRepo.GetDeactivatedPermissions();

    /// <summary>
    /// Purpose: Fetches all active roles.
    /// </summary>
    /// <returns></returns>
    public List<RoleDto>? GetActiveRoles() => _permRepo.GetActiveRoles();

    /// <summary>
    /// Purpose: Fetches all inactive roles.
    /// </summary>
    /// <returns></returns>
    public List<RoleDto>? GetDeactivatedRoles() => _permRepo.GetDeactivatedRoles();

    /// <summary>
    /// Updates the active status of a Role options.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public bool ToggleRoleStatus(int roleId) => roleId >= 0 && _permRepo.ToggleRoleStatus(roleId);

    /// <summary>
    /// Updates the active status of a Permission options.
    /// </summary>
    /// <param name="permId"></param>
    /// <returns></returns>
    public bool TogglePermissionStatus(int permId) => permId >= 0 && _permRepo.TogglePermissionStatus(permId);

    /// <summary>
    /// Adds a new role.
    /// </summary>
    /// <param name="newRole"></param>
    /// <returns></returns>
    public bool AddRole(RoleDto newRole, string username) =>
    !string.IsNullOrEmpty(newRole.Name) && IsAuthorized(username,"AddRole") && _permRepo.AddRole(newRole);

    /// <summary>
    /// Adds a new permission.
    /// </summary>
    /// <param name="newPerm"></param>
    /// <returns></returns>
    public bool AddPermission(PermissionDto newPerm, string username) =>
    !string.IsNullOrEmpty(newPerm.Name) && IsAuthorized(username, "AddPermission") && _permRepo.AddPermission(newPerm);

    /// <summary>
    /// Purpose: Assigns a permission to a role, enabling the role to perform some function in the
    /// system.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="permission"></param>
    /// <returns></returns>
    public bool AssignPermission(string role, string permission, string username) =>
        _permRepo.GetRoleByName(role) is RoleDto r &&
        _permRepo.GetPermissionByName(permission) is PermissionDto p &&
        IsAuthorized(username,"AssignPermission") &&
        _permRepo.AssignPermission(r, p);

    /// <summary>
    /// Unassigns a permission from a role.
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="role"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    public bool UnassignPermission(string permission, string role, string username) =>
        !string.IsNullOrEmpty(permission) && !string.IsNullOrEmpty(role) && !string.IsNullOrEmpty(username) &&
        IsAuthorized(username, "UnassignPermission") && _permRepo.UnassignPermission(permission, role);

    /// <summary>
    /// Fetches all permissions assigned to a user.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public List<PermissionDto>? GetAssignedPermissionsByUsername(string username) =>
        !string.IsNullOrEmpty(username) && _permRepo.GetAssignedPermissionsByUsername(username) is List<PermissionDto> perms ? perms : null;

    /// <summary>
    /// Fetches all permissions assigned to a role.
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public List<PermissionDto>? GetAssignedPermissionsByRole(string role) =>
        !string.IsNullOrEmpty(role) && _permRepo.GetAssignedPermissionsByRole(role) is List<PermissionDto> perms
        ? perms : null;

    /// <summary>
    /// Checks if a user is authorized to perform the requested function.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="permissionName"></param>
    /// <returns></returns>
    public bool IsAuthorized(string username, string permissionName)
    {
        if (GetAssignedPermissionsByUsername(username) is List<PermissionDto> permissions)
        {
            foreach (PermissionDto permission in permissions)
            {
                if (permission.Name == permissionName)
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns a role with the given ID.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public RoleDto? GetRole(int roleId) => _permRepo.GetRoleById(roleId) ?? null;

        /// <summary>
    /// Fetches a role with the given name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public RoleDto? GetRole(string name)
    {
        if (!string.IsNullOrEmpty(name) && _permRepo.GetRole(name) is Role role)
        {
            return new()
            {
                Id = role.Id
                ,
                Name = role.Name
                ,
                IsActive = role.IsActive
            };
        }
        return null;
    }

    /// <summary>
    /// Fetches a permission with the given name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public PermissionDto? GetPermission(string name)
    {
        if (!string.IsNullOrEmpty(name) && _permRepo.GetPermission(name) is Permission perm)
        {
            return new()
            {
                Id = perm.Id
                ,
                Name = perm.Name
                ,
                IsActive = perm.IsActive
                ,
                Description = perm.Description
            };
        }
        
        return null;
    }
}