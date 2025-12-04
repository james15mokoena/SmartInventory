using Microsoft.AspNetCore.Mvc;
using SmartInventory.API.Domain.DTO;
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
    /// <param name="username"></param>
    /// <param name="reason"></param>
    /// <param name="isNewProduct">Indicates whether a new product is added.</param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult RecordIncomingStock(string sku, int quantity, string username, string reason, bool isNewProduct) =>
            _stockService.RecordIncomingStock(sku, quantity, username, reason, isNewProduct) ?
            Ok("Stock transaction recorded successfully!") :
            BadRequest("Failed to record stock transaction!");

    /// <summary>
    /// Fetches stock transactions.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ViewStockTransactions() => _stockService.GetStockTransactions() is List<StockTransactionDto> dtos ?
                                                    Ok(dtos) :
                                                    BadRequest("Failed to fetch stock transactions!");

    /// <summary>
    /// Records an outgoing stock transaction.
    /// </summary>
    /// <param name="sku"></param>
    /// <param name="quantity"></param>
    /// <param name="username"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult RecordOutgoingStock(string sku, int quantity, string username, string reason) =>
                                            _stockService.RecordOutgoingStock(sku, quantity, username, reason) ?
                                            Ok("Stock transaction recorded successfully!") :
                                            BadRequest("Failed to record stock transaction!");
}