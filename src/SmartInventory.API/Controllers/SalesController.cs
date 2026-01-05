using Microsoft.AspNetCore.Mvc;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Services;

namespace SmartInventory.API.Controllers;

/// <summary>
/// Handles requests sent to the Sales subsystem.
/// </summary>
[Route("[controller]/[action]")]
[ApiController]
public class SalesController(SalesManagementService salesServ) : ControllerBase
{
    /// <summary>
    /// Used to interact with the sales subsystem.
    /// </summary>
    private readonly SalesManagementService _salesServ = salesServ;

    /// <summary>
    /// Generates a Requisition form and sends stores it in the user's computer/device.
    /// </summary>
    /// <param name="formData"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult GenerateRequisitionForm(RequisitionFormData formData) =>
        !string.IsNullOrEmpty(formData.Authorized) && !string.IsNullOrEmpty(formData.CompanyName) && !string.IsNullOrEmpty(formData.DocName) &&
        !string.IsNullOrEmpty(formData.CompanyAddress) && formData.DocNo >= 0 && !string.IsNullOrEmpty(formData.FromDepartment) ?
        CreatedAtAction(nameof(GenerateRequisitionForm),formData) : BadRequest("Failed to generate Requisition form!");
}