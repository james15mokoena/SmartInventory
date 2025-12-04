using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

/// <summary>
/// 
/// </summary>
/// <param name="stockRepo"></param>
public class StockManagementService(StockManagementRepository stockRepo, UserManagementService userService)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly StockManagementRepository _stockRepo = stockRepo;

    /// <summary>
    /// Used to interact with the user management service.
    /// </summary>
    private readonly UserManagementService _userService = userService;

    /// <summary>
    /// Used to record a stock transaction that adds stocks.
    /// </summary>
    /// <param name="sku">A product's stock-keeping unit number.</param>
    /// <param name="quantity">The quantity to be added to product.</param>
    /// <param name="username">An identifier for the user who initiated the transaction.</param>
    /// <param name="reason">The reason for which the transaction was initiated.</param>
    /// <param name="isNewProduct">Indicates whether a new product is added.</param>
    /// <returns></returns>
    public bool RecordIncomingStock(string sku, int quantity, string username, string reason, bool isNewProduct)
    {
        if (!string.IsNullOrEmpty(sku) && quantity > 0 && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(reason))
        {
            if(_userService.GetAdmin(username) is Admin admin)
                return _stockRepo.RecordIncomingStock(sku, quantity, admin.Id, reason, isNewProduct);
        }
            
        return false;
    }

    /// <summary>
    /// Used to deduct the specified quantity from the stock quantity.
    /// </summary>
    /// <param name="sku"></param>
    /// <param name="quantity"></param>
    /// <param name="username"></param>
    /// <param name="reasonType"></param>
    /// <returns></returns>
    public bool RecordOutgoingStock(string sku, int quantity, string username, string reason)
    {
        if (!string.IsNullOrEmpty(sku) && quantity > 0 && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(reason))
        {
            if(_userService.GetAdmin(username) is Admin admin)      
                return _stockRepo.RecordOutgoingStock(sku, quantity, admin.Id, reason);
        }
        return false;
    }

    /// <summary>
    /// Used to add a new transaction reason.
    /// </summary>
    /// <param name="reasonType"></param>
    /// <returns></returns>
    public bool AddTransactionReason(ReasonType reasonType) => !string.IsNullOrEmpty(reasonType.Reason) && _stockRepo.AddTransactionReason(reasonType);

    /// <summary>
    /// Used to delete a transaction reason.
    /// </summary>
    /// <param name="reasonTypeId"></param>
    /// <returns></returns>
    public bool DeleteTransactionReason(int reasonTypeId) => reasonTypeId >= 0 && _stockRepo.DeleteTransactionReason(reasonTypeId);

    /// <summary>
    /// Used to get all transaction reasons.
    /// </summary>
    /// <returns></returns>
    public List<ReasonType?>? GetTransactionReasons() => _stockRepo.GetTransactionReasons();

    /// <summary>
    /// Used to fetch all stock transactions.
    /// </summary>
    /// <returns></returns>
    public List<StockTransactionDto>? GetStockTransactions()
    {
        List<StockTransaction>? stockTransactions = _stockRepo.GetStockTransactions();

        if (stockTransactions != null && stockTransactions.Count > 0)
        {
            List<StockTransactionDto> stockTransactionDtos = [];
            foreach (StockTransaction stockTransaction in stockTransactions)
                stockTransactionDtos.Add(ToStockTransactionDto(stockTransaction));

            return stockTransactionDtos;
        }
        return null;
    }

     /// <summary>
    /// Used to fetch a product's stock transactions.
    /// </summary>
    /// <returns></returns>
    public List<StockTransactionDto>? GetStockTransactionsBySku(string sku)
    {
        List<StockTransaction>? stockTransactions = _stockRepo.GetStockTransactionsBySku(sku);

        if (stockTransactions != null && stockTransactions.Count > 0)
        {
            List<StockTransactionDto> stockTransactionDtos = [];
            foreach (StockTransaction stockTransaction in stockTransactions)
                stockTransactionDtos.Add(ToStockTransactionDto(stockTransaction));

            return stockTransactionDtos;
        }
        return null;
    }

    /// <summary>
    /// Used to adjust a product's stock.
    /// </summary>
    /// <param name="sku"></param>
    /// <param name="quantity"></param>
    /// <param name="username"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public bool RecordStockAdjustment(string sku, int quantity, string username, string reason)
    {
        if (!string.IsNullOrEmpty(sku) && quantity > 0 && !string.IsNullOrEmpty(username) && _userService.GetAdmin(username) is Admin admin &&
            !string.IsNullOrEmpty(reason))
            return _stockRepo.RecordStockAdjustment(sku, quantity, admin.Id, reason);
        return false;
    }

    /// <summary>
    /// Converts a StockTransaction object to StockTransactionDto object.
    /// </summary>
    /// <param name="stockTransaction"></param>
    /// <returns></returns>
    private StockTransactionDto ToStockTransactionDto(StockTransaction stockTransaction)
    {
        return new StockTransactionDto
        {
            TransactionId = stockTransaction.TransactionId
            ,UserId = stockTransaction.UserId
            ,ProductId = stockTransaction.ProductId
            ,NewStock = stockTransaction.NewStock
            ,PreviousStock = stockTransaction.PreviousStock
            ,QuantityChange = stockTransaction.QuantityChange
            ,Date = stockTransaction.Date
            ,ReasonTypeId = stockTransaction.ReasonTypeId
            ,Reason = _stockRepo.GetTransactionReason(stockTransaction.ReasonTypeId)
        };
    }
}