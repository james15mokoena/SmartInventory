using SmartInventory.API.Data;
using SmartInventory.API.Domain.Models;

namespace SmartInventory.API.Repositories;

/// <summary>
/// Defines the functionality for interacting with the database.
/// </summary>
public class StockManagementRepository(DatabaseContext context)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly DatabaseContext _context = context;

    /// <summary>
    /// Used to record a stock transaction that adds stocks.
    /// </summary>
    /// <param name="sku">A product's stock-keeping unit number.</param>
    /// <param name="quantity">The quantity to be added to product.</param>
    /// <param name="userId">An identifier for the user who initiated the transaction.</param>
    /// <param name="reasonTypeId">The reason for which the transaction was initiated.</param>
    /// <returns></returns>
    public bool RecordIncomingStock(string sku, int quantity, int userId, int reasonTypeId)
    {
        if (_context.Products.FirstOrDefault(s => s.SKU == sku) is Product stock)
        {
            // get the reason.
            if (_context.ReasonTypes.FirstOrDefault(r => r.Id == reasonTypeId) is ReasonType reasonType)
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
                    PreviousStock = stock.CurrentStock
                    ,
                    NewStock = stock.CurrentStock + quantity
                    ,
                    QuantityChange = quantity
                    ,
                    ReasonTypeId = reasonType.Id
                    ,
                    TransactionId = 0
                };

                // add more stocks.
                stock.CurrentStock += quantity;

                _context.Update(stock);
                if (_context.SaveChanges() > 0)
                {
                    _context.StockTransactions.Add(transaction);
                    return _context.SaveChanges() > 0;
                }
            }
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
    public List<ReasonType?>? GetTransactionReasons() => [.. _context.ReasonTypes.DefaultIfEmpty()];
}