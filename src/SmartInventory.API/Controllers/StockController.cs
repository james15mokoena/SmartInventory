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
    [HttpPost("{username}")]
    public IActionResult AddTransactionReason(ReasonType reasonType, string username) =>
        _stockService.AddTransactionReason(reasonType,username) ?
        CreatedAtAction(nameof(AddTransactionReason), reasonType) :
        BadRequest("Failed to add a new transaction reason!");

    /// <summary>
    /// Activates or deactivate a transaction reason.
    /// </summary>
    /// <param name="reasonType"></param>
    /// <returns></returns>
    [HttpPut("{reasonTypeId}")]
    public IActionResult ToggleTransactionReasonStatus(int reasonTypeId) => _stockService.ToggleTransactionReasonStatus(reasonTypeId) ?
                                                                        Ok("Transaction reason status updated successfully!") :
                                                                        BadRequest("Failed to update the transaction reason's status!");

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
    /// Fetches a stock's transactions.
    /// </summary>
    /// <returns></returns>
    [HttpGet("{sku}")]
    public IActionResult ViewStockTransactionsBySku(string sku) =>
                                            _stockService.GetStockTransactionsBySku(sku) is List<StockTransactionDto> dtos ?
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

        /// <summary>
        /// Adjusts a stock's quantity.
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="quantity"></param>
        /// <param name="username"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RecordStockAdjustment(string sku, int quantity, string username, string reason) =>
                                                _stockService.RecordStockAdjustment(sku, quantity, username, reason) ?
                                                Ok("Stock adjusted successfully!") :
                                                BadRequest("Failed to adjust stock!");

        /// <summary>
        /// Generates a stock report showing all the stocks and their quantities among other information.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        [HttpGet("{company}/{signature}")]
        public IActionResult GetStockReport(string company, string signature) =>
                !string.IsNullOrEmpty(company) && !string.IsNullOrEmpty(signature) &&
                _stockService.GetStockReport(company, signature) is StockReport stockReport ?
                Ok(stockReport) : BadRequest("Failed to generate the stock report!");

}