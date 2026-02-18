using Microsoft.AspNetCore.Mvc;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Services;

namespace SmartInventory.API.Controllers;

/// <summary>
/// Handles user requests to the user management subsystem.
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class UserController(UserManagementService uService) : ControllerBase
{

    /// <summary>
    /// Provides functionality for interacting with the user management subsystem.
    /// </summary>
    private readonly UserManagementService _userService = uService;

    /// <summary>
    /// Creates a new staff member.
    /// </summary>
    /// <param name="staff"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateStaffMember(Staff staff, string role) => _userService.CreateUser(staff,role) ?
                                                           CreatedAtAction(nameof(CreateStaffMember), staff) :
                                                           BadRequest("Failed to create staff member!");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Login(LoginDto login) => _userService.CheckUserExistsByUsernameAndPassword(login) is RoleDto role ?
                                                  Ok(role) :
                                                  BadRequest("Failed to loggin!");

    /// <summary>
    /// Activates or deactivates user (admin/staff).
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpPut("{username}")]
    public IActionResult ActivateOrDeactivateUser(string username) => _userService.ToggleUserActivation(username) ?
                                                                      Ok("Active status changed!") :
                                                                      BadRequest("Failed to change active status!");

    /// <summary>
    /// Gets a staff memmber with the given username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult ViewStaffMember(string username) => _userService.GetStaffMember(username) is Staff staff ?
                                                       Ok(staff) :
                                                       BadRequest("Failed to get the staff member!");

    /// <summary>
    /// Get all activated staff members.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ViewActivatedStaff() => _userService.GetActivatedStaff() is List<UserDto> staff ?
                                                 Ok(staff) :
                                                 BadRequest("Failed to get activated staff members!");

    /// <summary>
    /// Get all deactivated staff members.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ViewDeactivatedStaff() => _userService.GetDeactivatedStaff() is List<UserDto> staff ?
                                                 Ok(staff) :
                                                 BadRequest("Failed to get deactivated staff members!");

    /// <summary>
    /// Edits a staff member's data.
    /// </summary>
    /// <param name="updatedStaffMember"></param>
    /// <returns></returns>
    [HttpPut]
    public IActionResult EditStaffMember(UserDto updatedStaffMember) => _userService.EditStaffMember(updatedStaffMember) is UserDto dto ?
                                                             Ok(dto) :
                                                             BadRequest("Failed to update staff member!");
}