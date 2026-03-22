using SmartInventory.API.Data;
using SmartInventory.API.Domain.DTO;
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
    public bool RecordIncomingStock(string sku, int quantity, int userId, string reason)
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
                PreviousStock = stock.CurrentStock
                ,
                NewStock = 0
                ,
                QuantityChange = quantity
                ,
                ReasonTypeId = reasonType.Id
                ,
                TransactionId = 0
            };

            int newQuantity = -1;

            if (reasonType.Reason == "Issued" || reasonType.Reason == "Damaged" || reasonType.Reason == "Returned")
                newQuantity = stock.CurrentStock - quantity;

            // stock is sold/damaged/returned
            if (newQuantity != -1)
            {
                transaction.NewStock = newQuantity;
                stock.CurrentStock -= quantity;
            }
            // stock is received
            else if (newQuantity == -1)
            {
                transaction.NewStock = stock.CurrentStock + quantity;
                stock.CurrentStock += quantity;
            }

            // save the change made to product's quantity.
            _context.Update(stock);
            _context.SaveChanges();

            // save the transaction
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
                ,
                IsActive = true
            });

            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Used to activate or deactivate a transaction reason.
    /// </summary>
    /// <param name="reasonTypeId"></param>
    /// <returns></returns>
    public bool ToggleTransactionReasonStatus(int reasonTypeId)
    {
        if (_context.ReasonTypes.FirstOrDefault(r => r.Id == reasonTypeId) is ReasonType reason)
        {
            reason.IsActive = !reason.IsActive;
            _context.Update(reason);
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
    /// Used to fetch a product's stock transactions.
    /// </summary>
    /// <returns></returns>
    public List<StockTransaction>? GetStockTransactionsBySku(string sku) =>
                                                                 [.. from stockTransaction in _context.StockTransactions
                                                                 where stockTransaction.ProductId == sku
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
        if (_context.Products.FirstOrDefault(s => s.SKU == sku) is Product stock && userId >= 0 && !string.IsNullOrEmpty(reason)
                && _context.ReasonTypes.FirstOrDefault(r => r.Reason == reason) is ReasonType reasonType)
        {
            if (stock.CurrentStock - quantity >= 0)
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

    /// <summary>
    /// Generates a stock report.
    /// </summary>
    /// <param name="company">The name of the generating company.</param>
    /// <param name="signature">An identifier for the person who generated the report.</param>
    /// <returns></returns>
    public StockReport? GetStockReport(string company, string signature)
    {
        List<Product>? stocks = [.. from stock in _context.Products
                                select stock];

        if (stocks.Count > 0)
        {
            StockReport report = new()
            {
                CompanyName = company
                ,Signature = signature
            };

            foreach (var stock in stocks)
            {
                StockReportItem item = new()
                {
                    Code = stock.SKU
                    ,
                    Category = stock.Category
                    ,
                    Name = stock.Name
                    ,
                    StockLevel = stock.CurrentStock
                    ,
                    ReorderLevel = stock.ReorderQuantity
                    ,
                    MaximumLevel = stock.MaximumStockLevel
                    ,
                    IsReorder = stock.CurrentStock < stock.MinimumStockLevel ? "Yes" : "No"
                };

                report.Items.Add(item);
            }

            return report;
        }        
        
        return null;
    }
}