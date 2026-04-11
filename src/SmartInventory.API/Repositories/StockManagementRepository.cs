using Microsoft.EntityFrameworkCore;
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
    /// Finds the longest period without sales from the given product's transactions.
    /// </summary>
    /// <param name="transactions"></param>
    /// <returns></returns>
    private static List<DateTime>? FindLongestPeriodWithoutSales(List<StockTransaction> transactions)
    {
        // the first day of the current month.
        DateTime startDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
        // the number of days between the longest period without sales.
        int numDays = 0;
        // the longest period without sales
        List<DateTime> longestPeriodWithoutSales = [];

        if (transactions.Count > 0)
        {
            for (int i = 0; i < transactions.Count; ++i)
            {
                StockTransaction st1 = transactions[i];
                StockTransaction? st2 = (i + 1 < transactions.Count) ? transactions[i + 1] : null;

                if (i == 0)
                {
                    numDays = Math.Abs(st1.Date.Day - startDate.Day);
                    longestPeriodWithoutSales.Add(startDate);
                    longestPeriodWithoutSales.Add(st1.Date);
                }
                else if (i > 0 && st2 != null)
                {
                    numDays = Math.Abs(st2.Date.Day - st1.Date.Day) > numDays ? Math.Abs(st2.Date.Day - st1.Date.Day) : numDays;
                    longestPeriodWithoutSales.Add(st1.Date);
                    longestPeriodWithoutSales.Add(st2.Date);
                }
                
                if(i == 0 && st2 != null && Math.Abs(st2.Date.Day - st1.Date.Day) > numDays)
                {
                    numDays = Math.Abs(st2.Date.Day - st1.Date.Day);
                    longestPeriodWithoutSales.Clear();
                    longestPeriodWithoutSales.Add(st1.Date);
                    longestPeriodWithoutSales.Add(st2.Date);
                }
            }

            return longestPeriodWithoutSales;
        }

        return null;
    }

    /// <summary>
    /// Finds the longest period with consecutive sales for a given product during the month.
    /// </summary>
    /// <param name="transactions"></param>
    /// <returns></returns>
    private static List<DateTime>? FindLongestPeriodWithConsecutiveSales(List<StockTransaction> transactions)
    {
        if(transactions.Count > 0)
        {
            List<DateTime> longestPeriodWithConsecutiveSales = [];
            int numDays = 1;
            // the start date of the longest period with consecutive sales
            DateTime startDate = DateTime.Now;
            int maxPeriod = 0;
            List<int> numConsecutiveDays = [];

            for(int i=0; i<transactions.Count;)
            {
                StockTransaction st1 = transactions[i];
                startDate = st1.Date;
                longestPeriodWithConsecutiveSales.Add(st1.Date);
                int j = i + 1;

                while (j < transactions.Count)
                {
                    if (st1.Date.AddDays(j) == transactions[j].Date)
                    {
                        if (startDate == longestPeriodWithConsecutiveSales[0])
                        {
                            numDays += 1;
                            maxPeriod = Math.Max(numDays, maxPeriod);                            
                            longestPeriodWithConsecutiveSales.Add(transactions[j].Date);
                        }
                    }
                    else // WORK ON THIS!!!!!!
                    {
                        // the new start date
                        startDate = transactions[j].Date;
                        numConsecutiveDays.Add(numDays);
                        i = j;
                        break;
                    }

                    ++j;
                }                

                
            }
        }

        return null;
    }

    /// <summary>
    /// Generates a summary of the stock transactions.
    /// </summary>
    /// <param name="reason">Used to specify which category of transaction reasons to
    /// fetch.</param>
    /// <returns></returns>
    public List<StockTransactionSummary>? GetStockTransactionsSummaries(string? reason)
    {

        if (!string.IsNullOrEmpty(reason) && _context.ReasonTypes.FirstOrDefault(r => r.Reason == reason) is ReasonType rType)
        {
            // get all transactions categorized by reason.
            List<StockTransaction>? transactions = [..
                from transaction in _context.StockTransactions
                where transaction.ReasonTypeId == rType.Id
                orderby transaction.ProductId, transaction.Date
                select transaction
            ];

            // summaries for each product's transactions
            List<StockTransactionSummary> transactionSummaries = [];

            // how many transactions categorized by reason does each product have?
            var numTransactionForEachProduct =
                from transaction in _context.StockTransactions
                where transaction.ReasonTypeId == rType.Id
                group transaction by transaction.ProductId into transactionGroup
                select new
                {
                    ProductId = transactionGroup.Key
                    ,
                    NumTransactions = transactionGroup.Count()
                };

            // the next index in the transactions list for when creating separate lists.
            int nextIndex = 0;

            for (int i = 0; i < numTransactionForEachProduct.Count(); ++i)
            {
                // create a summary for the product's transactions
                StockTransactionSummary summary = new();

                // stores the longest period without sales for the current product.
                List<DateTime> longestPeriodWithoutSales = [];

                // contains transactions for a single product.
                List<StockTransaction> stockTransactions = [];

                // copy the transactions
                stockTransactions = transactions.Slice(nextIndex, numTransactionForEachProduct.ElementAt(i).NumTransactions);
                // update next index to refer to the next transaction is list.
                nextIndex += numTransactionForEachProduct.ElementAt(i).NumTransactions;

                // find the longest period without sales
                longestPeriodWithoutSales = FindLongestPeriodWithoutSales(stockTransactions)!;

                // compile the summary
                summary.ProductName = _context.Products.FirstOrDefault(p => p.SKU == numTransactionForEachProduct.ElementAt(i).ProductId)!.Name;
                summary.StartOfLongestPeriodWithoutSales = longestPeriodWithoutSales.ElementAt(0);
                summary.EndOfLongestPeriodWithoutSales = longestPeriodWithoutSales.ElementAt(1);

                // add the summary to the list
                transactionSummaries.Add(summary);
            }

            return transactionSummaries;
        }

        return null;
    }

    /// <summary>
    /// Used to fetch all stock transactions.
    /// <param name="start">Indicates where to start reading.</param>
    /// <param name="count">The number of records to fetch at a time.</param>
    /// </summary>
    /// <returns></returns>
    public List<StockTransaction>? GetStockTransactions(int start = 0, int count = 15) =>
            [.. from transaction in _context.StockTransactions.Skip(start).Take(count)
            select transaction];

    /// <summary>
    /// Used to fetch a product's stock transactions.
    /// </summary>
    /// <returns></returns>
    public List<StockTransaction>? GetStockTransactionsBySku(string sku) =>
                                                                 [.. from stockTransaction in _context.StockTransactions
                                                                 where stockTransaction.ProductId == sku
                                                                 select stockTransaction];

    /// <summary>
    /// Fetches transaction classified by the specified "reason".
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    public List<StockTransaction>? GetStockTransactionsByReason(string reason) =>
        [.. from transaction in _context.StockTransactions
        where transaction.ReasonTypeId == GetTransactionReasonId(reason)
        select transaction];

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

            if (stock.CurrentStock + quantity >= 0)
            {
                // record the transaction
                _context.StockTransactions.Add(new()
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
                });
            }

            // increase the stock's quantity
            stock.CurrentStock += quantity;
            _context.Products.Update(stock);
            return _context.SaveChanges() > 0;
        }

        return false;
    }

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
                , Signature = signature
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

/// <summary>
/// Stores the number of transactions that a product has.
/// </summary>
class ProductTransactionCount
{
    /// <summary>
    /// The product's SKU.
    /// </summary>
    public string ProductId { get; set; } = "";

    /// <summary>
    /// The number of transactions.
    /// </summary>
    public int NumTransactions { get; set; }
}