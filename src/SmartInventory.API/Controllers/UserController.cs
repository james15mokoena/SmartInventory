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
    /// Creates a new admin.
    /// </summary>
    /// <param name="admin"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateAdmin(Admin admin) => _userService.CreateUser(admin) ?
                                                    CreatedAtAction(nameof(CreateAdmin),admin) :
                                                    BadRequest("Failed to create admin!");

    /// <summary>
    /// Creates a new staff member.
    /// </summary>
    /// <param name="staff"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateStaffMember(Staff staff) => _userService.CreateUser(staff) ?
                                                           CreatedAtAction(nameof(CreateStaffMember),staff) :
                                                           BadRequest("Failed to create staff member!");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpGet("{username}/{password}")]
    public IActionResult Login(string username, string password) => _userService.CheckUserExistsByUsernameAndPassword(username, password) ?
                                                                    Ok("Logged in!") : BadRequest("Failed to loggin!");

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
    /// Gets an adminstrator with the given username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult ViewAdmin(string username) => _userService.GetAdmin(username) is Admin admin ?
                                                       Ok(admin) :
                                                       BadRequest("Failed to get the admin!");

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
    /// Get all active administrators.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ViewActivatedAdmins() => _userService.GetActivatedAdmins() is List<Admin> admins ?
                                                Ok(admins) :
                                                BadRequest("Failed to get active administrators!");

    /// <summary>
    /// Get all deactivated administrators.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ViewDeactivatedAdmins() => _userService.GetDeactivatedAdmins() is List<Admin> admins ?
                                                    Ok(admins) :
                                                    BadRequest("Failed to get deactivated administrators!");

    /// <summary>
    /// Get all activated staff members.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ViewActivatedStaff() => _userService.GetActivatedStaff() is List<Staff> staff ?
                                                 Ok(staff) :
                                                 BadRequest("Failed to get activated staff members!");

    /// <summary>
    /// Get all deactivated staff members.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ViewDeactivatedStaff() => _userService.GetDeactivatedStaff() is List<Staff> staff ?
                                                 Ok(staff) :
                                                 BadRequest("Failed to get deactivated staff members!");

    /// <summary>
    /// Edits admin's data.
    /// </summary>
    /// <param name="updatedAdmin"></param>
    /// <returns></returns>
    [HttpPut]
    public IActionResult EditAdmin(AdminDto updatedAdmin) => _userService.EditAdmin(updatedAdmin) is AdminDto dto ?
                                                             Ok(dto) :
                                                             BadRequest("Failed to update admin!");
}