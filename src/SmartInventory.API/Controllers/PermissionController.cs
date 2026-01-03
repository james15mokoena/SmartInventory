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
    public IActionResult GetInActivePermissions() => _permService.GetInActivePermissions() is List<PermissionDto> perms ?
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
    public IActionResult GetInActiveRoles() => _permService.GetInActiveRoles() is List<RoleDto> roles ?
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
    public IActionResult TogglePermissionStatus(int permId) => permId >= 0 && _permService.TogglePermissionStatus(permId) ?
                                                        Ok("Permission status is updated successfully!") :
                                                        BadRequest("Failed to update permission status!");

    /// <summary>
    /// Adds a new role.
    /// </summary>
    /// <param name="newRole"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult AddRole(RoleDto newRole) => !string.IsNullOrEmpty(newRole.Name) && _permService.AddRole(newRole) ?
                                                    Ok(newRole) :
                                                    BadRequest("Failed to add a new role.");

    /// <summary>
    /// Adds a new permission.
    /// </summary>
    /// <param name="newPerm"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult AddPermission(PermissionDto newPerm) => !string.IsNullOrEmpty(newPerm.Name) && _permService.AddPermission(newPerm) ?
                                                    Ok(newPerm) :
                                                    BadRequest("Failed to add a new permission.");
}