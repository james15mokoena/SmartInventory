using Microsoft.AspNetCore.Mvc;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Services;

namespace SmartInventory.API.Controllers;

/// <summary>
/// Handles requests to the stock management subsystem.
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class StockController(StockManagementService stockService) : ControllerBase
{
    /// <summary>
    /// Used to interact with the stock management subsystem.
    /// </summary>
    private readonly StockManagementService _stockService = stockService;

    /// <summary>
    /// Adds a new transaction reason.
    /// </summary>
    /// <param name="reasonType"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult AddTransactionReason(ReasonType reasonType) => _stockService.AddTransactionReason(reasonType) ?
                                                                        CreatedAtAction(nameof(AddTransactionReason),reasonType) :
                                                                        BadRequest("Failed to add a new transaction!");
}