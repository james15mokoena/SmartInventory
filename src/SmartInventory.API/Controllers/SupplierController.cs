using Microsoft.AspNetCore.Mvc;
using SmartInventory.API.Domain.DTO;
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
    public IActionResult CreateSupplier(SupplierDto supplier) => _suppService.CreateSupplier(supplier) ?
                                                              CreatedAtAction(nameof(CreateSupplier), supplier) :
                                                              BadRequest("Failed to add a new supplier!");

    /// <summary>
    /// Gets a supplier with the given ID.
    /// </summary>
    /// <param name="supplierNo"></param>
    /// <returns></returns>
    [HttpGet("{supplierNo}")]
    public IActionResult ViewSupplier(int supplierNo) => _suppService.GetSupplier(supplierNo) is Supplier supplier ?
                                                         Ok(supplier) :
                                                         BadRequest("Failed to get the supplier's data.");

    /// <summary>
    /// Activates or deactivates supplier's active status.
    /// </summary>
    /// <param name="supplierNo"></param>
    /// <returns></returns>
    [HttpPut("{supplierNo}")]
    public IActionResult ActivateOrDeactivateSupplier(int supplierNo) => _suppService.ToggleSupplierActiveStatus(supplierNo) ?
                                                        Ok("Active status changed!") :
                                                        BadRequest("Failed to update supplier's active status!");

    /// <summary>
    /// Gets all active suppliers.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ViewActivatedSuppliers() => _suppService.GetActiveSuppliers() is List<Supplier> suppliers ?
                                                     Ok(suppliers) :
                                                     BadRequest("Failed to fetch active suppliers!");

    /// <summary>
    /// Gets all deactivated suppliers.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ViewDeactivatedSuppliers() => _suppService.GetDeactivatedSuppliers() is List<Supplier> suppliers ?
                                                     Ok(suppliers) :
                                                     BadRequest("Failed to fetch deactivated suppliers!");

    /// <summary>
    /// Edits a supplier's data.
    /// </summary>
    /// <param name="updatedSupplier"></param>
    /// <returns></returns>
    [HttpPut]
    public IActionResult EditSupplier(SupplierDto updatedSupplier) => _suppService.EditSupplier(updatedSupplier) is SupplierDto dto ?
                                                                      Ok(dto) :
                                                                      BadRequest("Failed to edit supplier's data.");
}