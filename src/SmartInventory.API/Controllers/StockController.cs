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
                                                                        CreatedAtAction(nameof(AddTransactionReason), reasonType) :
                                                                        BadRequest("Failed to add a new transaction reason!");

    /// <summary>
    /// Deletes a transaction reason.
    /// </summary>
    /// <param name="reasonType"></param>
    /// <returns></returns>
    [HttpDelete("{reasonTypeId}")]
    public IActionResult DeleteTransactionReason(int reasonTypeId) => _stockService.DeleteTransactionReason(reasonTypeId) ?
                                                                        Ok("Transaction reason deleted successfully!") :
                                                                        BadRequest("Failed to delete a transaction reason!");

    /// <summary>
    /// Fetches transaction reasons.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ViewTransactionReasons() => _stockService.GetTransactionReasons() is List<ReasonType?> reasonTypes ?
                                                     Ok(reasonTypes) :
                                                     BadRequest("Failed to fetch transaction reasons!");

    /// <summary>
    /// Records a new stock transaction.
    /// </summary>
    /// <param name="sku"></param>
    /// <param name="quantity"></param>
    /// <param name="userId"></param>
    /// <param name="reasonTypeId"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult RecordIncomingStock(string sku, int quantity, int userId, int reasonTypeId) =>
            _stockService.RecordIncomingStock(sku, quantity, userId, reasonTypeId) ?
            Ok("Stock transaction recorded successfully!") :
            BadRequest("Failed to record stock transaction!");
}