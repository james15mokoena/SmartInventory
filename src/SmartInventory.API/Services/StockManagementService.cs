using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

/// <summary>
/// 
/// </summary>
/// <param name="stockRepo"></param>
public class StockManagementService(StockManagementRepository stockRepo)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly StockManagementRepository _stockRepo = stockRepo;

    /// <summary>
    /// Used to record a stock transaction that adds stocks.
    /// </summary>
    /// <param name="sku">A product's stock-keeping unit number.</param>
    /// <param name="quantity">The quantity to be added to product.</param>
    /// <param name="userId">An identifier for the user who initiated the transaction.</param>
    /// <param name="reasonTypeId">The reason for which the transaction was initiated.</param>
    /// <param name="isNewProduct">Indicates whether a new product is added.</param>
    /// <returns></returns>
    public bool RecordIncomingStock(string sku, int quantity, int userId, int reasonTypeId, bool isNewProduct)
    {
        if (!string.IsNullOrEmpty(sku) && quantity > 0 && userId >= 0 && reasonTypeId >= 0)
            return _stockRepo.RecordIncomingStock(sku, quantity, userId, reasonTypeId, isNewProduct);
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
    /// Converts a StockTransaction object to StockTransactionDto object.
    /// </summary>
    /// <param name="stockTransaction"></param>
    /// <returns></returns>
    private static StockTransactionDto ToStockTransactionDto(StockTransaction stockTransaction)
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
        };
    }
}