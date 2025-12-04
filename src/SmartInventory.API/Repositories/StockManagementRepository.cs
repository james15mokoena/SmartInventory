using SmartInventory.API.Data;
using SmartInventory.API.Domain.Models;

namespace SmartInventory.API.Repositories;

/// <summary>
/// Defines the functionality for interacting with the database.
/// </summary>
public class StockManagementRepository(DatabaseContext context, UserManagementRepository userRepo)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly DatabaseContext _context = context;

    /// <summary>
    /// Used to interact with the user management subsystem.
    /// </summary>
    private readonly UserManagementRepository _userRepo = userRepo;

    /// <summary>
    /// Used to record a stock transaction that adds stocks.
    /// </summary>
    /// <param name="sku">A product's stock-keeping unit number.</param>
    /// <param name="quantity">The quantity to be added to product.</param>
    /// <param name="userId">An identifier for the user who initiated the transaction.</param>
    /// <param name="reason">The reason for which the transaction was initiated.</param>
    /// <param name="isNewProduct">Indicates whether a new product is added.</param>
    /// <returns></returns>
    public bool RecordIncomingStock(string sku, int quantity, int userId, string reason, bool isNewProduct)
    {
        if (_context.Products.FirstOrDefault(s => s.SKU == sku) is Product stock && !string.IsNullOrEmpty(reason) &&
            _context.ReasonTypes.FirstOrDefault(r => r.Reason == reason) is ReasonType reasonType)
        {
            // record the transaction
            StockTransaction transaction = new()
            {
                UserId = userId
                ,
                Date = DateTime.Now
                ,
                ProductId = sku
                ,
                Product = stock
                ,
                PreviousStock = isNewProduct ? 0 : stock.CurrentStock
                ,
                NewStock = isNewProduct ? quantity : stock.CurrentStock + quantity
                ,
                QuantityChange = quantity
                ,
                ReasonTypeId = reasonType.Id
                ,
                TransactionId = 0
            };

            // add more stocks, if the product already exists.
            if (!isNewProduct)
            {
                stock.CurrentStock += quantity;
                _context.Update(stock);
                _context.SaveChanges();
            }

            _context.StockTransactions.Add(transaction);
            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Used to add a new transaction reason.
    /// </summary>
    /// <param name="reasonType"></param>
    /// <returns></returns>
    public bool AddTransactionReason(ReasonType reasonType)
    {
        if (_context.ReasonTypes.FirstOrDefault(r => r.Reason == reasonType.Reason) == null)
        {
            _context.ReasonTypes.Add(new()
            {
                Id = 0
                ,
                Reason = reasonType.Reason
            });

            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Used to delete a transaction reason.
    /// </summary>
    /// <param name="reasonTypeId"></param>
    /// <returns></returns>
    public bool DeleteTransactionReason(int reasonTypeId)
    {
        if (_context.ReasonTypes.FirstOrDefault(r => r.Id == reasonTypeId) is ReasonType reason)
        {
            _context.ReasonTypes.Remove(reason);
            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Used to fetch all transaction reasons.
    /// </summary>
    /// <returns></returns>
    public List<ReasonType?>? GetTransactionReasons() => [.. from transactionReason in _context.ReasonTypes
                                                             select transactionReason];

    /// <summary>
    /// Used to fetch the ID of the passed transaction reason.
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    public int GetTransactionReasonId(string reason) => (from transReason in _context.ReasonTypes
                                                         where transReason.Reason == reason
                                                         select transReason).First().Id;

    /// <summary>
    /// Used to fetch the reason for the stock transaction made.
    /// </summary>
    /// <param name="reasonId"></param>
    /// <returns></returns>
    public string GetTransactionReason(int reasonId) => (from transReason in _context.ReasonTypes
                                                         where transReason.Id == reasonId
                                                         select transReason).First().Reason;

    /// <summary>
    /// Used to fetch all stock transactions.
    /// </summary>
    /// <returns></returns>
    public List<StockTransaction>? GetStockTransactions() => [.. from stockTransaction in _context.StockTransactions
                                                                 select stockTransaction];

    /// <summary>
    /// Used to deduct the specified quantity from the stock quantity.
    /// </summary>
    /// <param name="sku"></param>
    /// <param name="quantity"></param>
    /// <param name="userId"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public bool RecordOutgoingStock(string sku, int quantity, int userId, string reason)
    {
        if(_context.Products.FirstOrDefault(s => s.SKU == sku) is Product stock && userId >= 0 && !string.IsNullOrEmpty(reason)
                && _context.ReasonTypes.FirstOrDefault(r => r.Reason == reason) is ReasonType reasonType)
        {
            if(stock.CurrentStock - quantity >= 0)
            {
                _context.StockTransactions.Add(new StockTransaction
                {
                    UserId = userId
                    ,
                    Date = DateTime.Now
                    ,
                    ProductId = stock.SKU
                    ,
                    Product = stock
                    ,
                    TransactionId = 0
                    ,
                    ReasonTypeId = reasonType.Id
                    ,
                    PreviousStock = stock.CurrentStock
                    ,
                    NewStock = stock.CurrentStock - quantity
                    ,
                    QuantityChange = quantity
                });

                // deduct the stock quantity.
                stock.CurrentStock -= quantity;
                _context.Products.Update(stock);
                return _context.SaveChanges() > 0;
            }
        }
        return false;
    }
}