using Microsoft.AspNetCore.Mvc;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Services;

namespace SmartInventory.API.Controllers;

/// <summary>
/// Handles requests to the procurement subsystem.
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class ProcurementController(ProcurementManagementService procServ) : ControllerBase
{
    private readonly ProcurementManagementService _procServ = procServ;

    /// <summary>
    /// Generates a quotation.
    /// </summary>
    /// <param name="quote"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult GenerateQuotation(QuotationDto quote) => _procServ.GenerateQuotation(quote) is QuotationDto dto ?
        Ok(dto) : BadRequest("Failed to generate the quotation!");
}