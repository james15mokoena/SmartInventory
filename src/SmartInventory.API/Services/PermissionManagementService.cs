using SmartInventory.API.Domain.DTO;
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
    public List<PermissionDto>? GetInActivePermissions() => _permRepo.GetInActivePermissions();

    /// <summary>
    /// Purpose: Fetches all active roles.
    /// </summary>
    /// <returns></returns>
    public List<RoleDto>? GetActiveRoles() => _permRepo.GetActiveRoles();

    /// <summary>
    /// Purpose: Fetches all inactive roles.
    /// </summary>
    /// <returns></returns>
    public List<RoleDto>? GetInActiveRoles() => _permRepo.GetInActiveRoles();

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
    public bool AddRole(RoleDto newRole) => !string.IsNullOrEmpty(newRole.Name) && _permRepo.AddRole(newRole);

    /// <summary>
    /// Adds a new permission.
    /// </summary>
    /// <param name="newPerm"></param>
    /// <returns></returns>
    public bool AddPermission(PermissionDto newPerm) => !string.IsNullOrEmpty(newPerm.Name) && _permRepo.AddPermission(newPerm);
}