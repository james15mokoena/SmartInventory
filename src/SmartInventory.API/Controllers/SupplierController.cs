using Microsoft.AspNetCore.Mvc;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Services;

namespace SmartInventory.API.Controllers;

/// <summary>
/// Handles requests to the supplier management subsystem.
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class SupplierController(SupplierManagementService suppService) : ControllerBase
{
    /// <summary>
    /// Enables interaction with the supplier management subsystem.
    /// </summary>
    private readonly SupplierManagementService _suppService = suppService;

    /// <summary>
    /// Adds a new supplier.
    /// </summary>
    /// <param name="supplier"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateSupplier(Supplier supplier) => _suppService.CreateSupplier(supplier) ?
                                                              CreatedAtAction(nameof(CreateSupplier), supplier) :
                                                              BadRequest("Failed to add a new supplier!");
}