using System.ComponentModel;
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
        // the longest period without sales
        List<DateTime> lpws = [];

        if (transactions.Count > 0)
        {
            int i = 0, j = i + 1;
            DateTime? startLpws = null;
            DateTime? endLpws = null;
            int maxPeriod = 0;

            StockTransaction t1 = transactions[i];
            DateTime startDate = FirstDateOfCurrentMonth();
            int dayDiff = t1.Date.Day - startDate.Day;

            if (dayDiff >= 2 && dayDiff > maxPeriod)
            {
                maxPeriod = dayDiff;
                startLpws = startDate;
                endLpws = t1.Date;
            }            
            
            while (i < transactions.Count && j < transactions.Count)
            {
                t1 = transactions[i];
                StockTransaction t2 = transactions[j];
                dayDiff = t2.Date.Day - t1.Date.Day;

                if (dayDiff >= 2 && dayDiff > maxPeriod)
                {
                    maxPeriod = dayDiff;
                    startLpws = t1.Date;
                    endLpws = t2.Date;
                }

                ++i;
                ++j;
            }

            if (startLpws != null && endLpws != null)
            {
                lpws.Add(new DateTime(startLpws.Value.Date.Year, startLpws.Value.Date.Month, startLpws.Value.Date.Day));
                lpws.Add(new DateTime(endLpws.Value.Date.Year, endLpws.Value.Date.Month, endLpws.Value.Date.Day));
            }
            else
            {
                lpws.Add(new DateTime(transactions[0].Date.Year, transactions[0].Date.Month, transactions[0].Date.Day));
                lpws.Add(new DateTime(transactions[0].Date.Year, transactions[0].Date.Month, transactions[0].Date.Day));             
            }
            
            return lpws;
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
        if(transactions.Count > 1)
        {
            int i = 0, j = i + 1;
            int idx = 1;
            // stores the longest period with consecutive sales.
            DateTime? startLpwcs = null;
            DateTime? endLpwcs = null;
            bool isDiscontinued = false;

            while (i < transactions.Count)
            {
                StockTransaction t1 = transactions[i];

                while (j < transactions.Count)
                {
                    StockTransaction t2 = transactions[j];
                    DateOnly t2Date = DateOnly.FromDateTime(t2.Date);
                    DateOnly nextDate = DateOnly.FromDateTime(t1.Date.AddDays(idx));                    

                    if (nextDate != t2Date)
                    {
                        if (startLpwcs == null)
                            startLpwcs = t1.Date;

                        if (endLpwcs == null)
                            endLpwcs = transactions[j - 1].Date;

                        if (Math.Abs(startLpwcs.Value.Day - endLpwcs.Value.Day) <= Math.Abs(t1.Date.Day - transactions[j - 1].Date.Day))
                        {
                            startLpwcs = t1.Date;
                            endLpwcs = transactions[j - 1].Date;
                        }

                        isDiscontinued = true;
                        break;
                    }

                    ++idx;
                    ++j;
                }

                if (isDiscontinued)
                {
                    i = j;
                    j = i + 1;
                    idx = 1;
                    isDiscontinued = false;
                }
                else
                    ++i;

                if (j >= transactions.Count)
                    break;
            }

            if(startLpwcs != null && endLpwcs != null)
            {
                List<DateTime> lpwcs = [];
                lpwcs.Add(new DateTime(startLpwcs.Value.Year, startLpwcs.Value.Month, startLpwcs.Value.Day));
                lpwcs.Add(new DateTime(endLpwcs.Value.Year, endLpwcs.Value.Month, endLpwcs.Value.Day));
                return lpwcs;
            }

        }
        else if (transactions.Count  == 1)
        {
            List<DateTime> lpwcs = [];
            lpwcs.Add(new DateTime(transactions[0].Date.Year, transactions[0].Date.Month, transactions[0].Date.Day));
            lpwcs.Add(new DateTime(transactions[0].Date.Year, transactions[0].Date.Month, transactions[0].Date.Day));
            return lpwcs;
        }

        return null;
    }

    /// <summary>
    /// Finds the first date of the current month.
    /// </summary>
    /// <returns></returns>
    private static DateTime FirstDateOfCurrentMonth() => DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1);
    
    /// <summary>
    /// Finds the last date of the current month.
    /// </summary>
    /// <returns></returns>
    private static DateTime LastDateOfCurrentMonth()
    {
        return DateTime.Now.Month switch
        {
            2                                   => DateTime.Now.AddDays(28 - DateTime.Now.Day),
            4 or 6 or 9 or 11                   => DateTime.Now.AddDays(30 - DateTime.Now.Day),
            1 or 3 or 5 or 7 or 8 or 10 or 12   => DateTime.Now.AddDays(31 - DateTime.Now.Day),
            _                                   => DateTime.Now,
        };
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
            // Which sales transactions occurred in the current month of the current year?
            List<StockTransaction>? transactions = [..
                from transaction in _context.StockTransactions
                where transaction.ReasonTypeId == rType.Id && transaction.Date >= FirstDateOfCurrentMonth() &&
                transaction.Date <= LastDateOfCurrentMonth()
                orderby transaction.ProductId, transaction.Date
                select transaction
            ];

            // summaries for each product's transactions
            List<StockTransactionSummary> transactionSummaries = [];

            // How many transactions categorized by reason does each product have?
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
                // stores the longes period with consecutive sales for the current product.
                List<DateTime> longestPeriodWithConsecutiveSales = [];

                // contains transactions for a single product.
                List<StockTransaction> stockTransactions = [];

                // copy the transactions
                stockTransactions = transactions.Slice(nextIndex, numTransactionForEachProduct.ElementAt(i).NumTransactions);
                // update next index to refer to the next transaction is list.
                nextIndex += numTransactionForEachProduct.ElementAt(i).NumTransactions;

                // find the longest period without sales
                longestPeriodWithoutSales = FindLongestPeriodWithoutSales(stockTransactions)!;
                // find the longest period with consecutive sales
                longestPeriodWithConsecutiveSales = FindLongestPeriodWithConsecutiveSales(stockTransactions)!;

                // compile the summary
                summary.ProductName = _context.Products.FirstOrDefault(p => p.SKU == numTransactionForEachProduct.ElementAt(i).ProductId)!.Name;
                summary.StartOfLongestPeriodWithoutSales = longestPeriodWithoutSales.ElementAt(0);
                summary.EndOfLongestPeriodWithoutSales = longestPeriodWithoutSales.ElementAt(1);
                summary.StartOfLongestPeriodWithConsecutiveSales = longestPeriodWithConsecutiveSales.ElementAt(0);
                summary.EndOfLongestPeriodWithConsecutiveSales = longestPeriodWithConsecutiveSales.ElementAt(1);

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