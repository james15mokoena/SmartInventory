using Microsoft.AspNetCore.Mvc;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Services;

namespace SmartInventory.API.Controllers;

/// <summary>
/// Purpose: Handles the requests sent to the permission subsystem.
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class PermissionController(PermissionManagementService perm) : ControllerBase
{
    private readonly PermissionManagementService _permService = perm;

    /// <summary>
    /// Fetches all active permissions.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetActivePermissions() => _permService.GetActivePermissions() is List<PermissionDto> perms ?
                                                   Ok(perms) :
                                                   BadRequest("Failed to fetch permissions!");

    /// <summary>
    /// Fetches all deactivated permissions.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetDeactivatedPermissions() => _permService.GetDeactivatedPermissions() is List<PermissionDto> perms ?
                                                   Ok(perms) :
                                                   BadRequest("Failed to fetch deactivated permissions!");

    /// <summary>
    /// Fetches all active roles.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetActiveRoles() => _permService.GetActiveRoles() is List<RoleDto> roles ?
                                                   Ok(roles) :
                                                   BadRequest("Failed to fetch roles!");

    /// Fetches all deactivated roles.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetDeactivatedRoles() => _permService.GetDeactivatedRoles() is List<RoleDto> roles ?
                                                   Ok(roles) :
                                                   BadRequest("Failed to fetch deactivated roles!");

    /// <summary>
    /// Updates the active status of a role option.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [HttpPut("{roleId}")]
    public IActionResult ToggleRoleStatus(int roleId) => roleId >= 0 && _permService.ToggleRoleStatus(roleId) ?
                                                        Ok("Role status is updated successfully!") :
                                                        BadRequest("Failed to update role status!");

    /// <summary>
    /// Updates the active status of a permission option.
    /// </summary>
    /// <param name="permId"></param>
    /// <returns></returns>
    [HttpPut("{permId}")]
    public IActionResult TogglePermissionStatus(int permId) =>
        permId >= 0 && _permService.TogglePermissionStatus(permId) ?
        Ok("Permission status is updated successfully!") :
        BadRequest("Failed to update permission status!");

    /// <summary>
    /// Adds a new role.
    /// </summary>
    /// <param name="newRole"></param>
    /// <returns></returns>
    [HttpPost("{username}")]
    public IActionResult AddRole(RoleDto newRole,string username) =>
        !string.IsNullOrEmpty(newRole.Name) && _permService.AddRole(newRole,username) ?
        Ok(newRole) :
        BadRequest("Failed to add a new role.");

    /// <summary>
    /// Adds a new permission.
    /// </summary>
    /// <param name="newPerm"></param>
    /// <returns></returns>
    [HttpPost("{username}")]
    public IActionResult AddPermission(PermissionDto newPerm, string username) =>
    !string.IsNullOrEmpty(newPerm.Name) && _permService.AddPermission(newPerm, username) ?
    Ok(newPerm) :
    BadRequest("Failed to add a new permission.");

    /// <summary>
    /// Assigns a permission to a role, enabling the role to perform some function in the system.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="permission"></param>
    /// <returns></returns>
    [HttpPost("{role}/{permission}/{username}")]
    public IActionResult AssignPermission(string role, string permission, string username) =>
        _permService.AssignPermission(role, permission,username) ?
        Ok("Permission assigned successfully!") :
        BadRequest("Failed to assign the permission to the role.");

    /// <summary>
    /// Unassigns a permission from a role.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="permission"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpDelete("{role}/{permission}/{username}")]
    public IActionResult UnassignPermission(string role, string permission, string username) =>
        _permService.UnassignPermission(permission, role, username) ?
        Ok("Permission unassigned successfully!") :
        BadRequest("Failed to unassign the permission from the role.");

    /// <summary>
    /// Fetches the permissions assigned to the user.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult GetAssignedPermissionsByUsername(string username) =>
        !string.IsNullOrEmpty(username) && _permService.GetAssignedPermissionsByUsername(username) is List<PermissionDto> perms ?
        Ok(perms) : BadRequest("Failed to fetched the permissions assigned to this user!");

    /// <summary>
    /// Fetches the permissions assigned to the role.
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpGet("{role}")]
    public IActionResult GetAssignedPermissionsByRole(string role) =>
        !string.IsNullOrEmpty(role) && _permService.GetAssignedPermissionsByRole(role) is List<PermissionDto> perms ?
        Ok(perms) : BadRequest("Failed to fetched the permissions assigned to this role!");

    /// <summary>
    /// Fetches a role with the given name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("{name}")]
    public IActionResult GetRole(string name) =>
        !string.IsNullOrEmpty(name) && _permService.GetRole(name) is RoleDto role ?
        Ok(role) : BadRequest("Failed to fetch the role with the given name!");

    /// <summary>
    /// Fetches a permission with the given name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("{name}")]
    public IActionResult GetPermission(string name) =>
        !string.IsNullOrEmpty(name) && _permService.GetPermission(name) is PermissionDto perm ?
        Ok(perm) : BadRequest("Failed to fetch the permission with the given name!");
}