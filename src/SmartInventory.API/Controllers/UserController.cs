using Microsoft.AspNetCore.Mvc;
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
    public IActionResult CreateAdmin(Admin admin) => _userService.CreateUser(admin) ? Ok("Admin created successfully!") : BadRequest("Failed to create admin!");

    /// <summary>
    /// Creates a new staff member.
    /// </summary>
    /// <param name="staff"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateStaffMember(Staff staff) => _userService.CreateUser(staff) ? Ok("Staff member created successfully!") : BadRequest("Failed to create staff member!");

    /// <summary>
    /// Creates a new supplier.
    /// </summary>
    /// <param name="supplier"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateSupplier(Supplier supplier) => _userService.CreateUser(supplier) ? Ok("Supplier created successfully!") : BadRequest("Failed to create supplier!");
}