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
    /// Given the sales transactions of a product, it finds the longest period without sales in the
    /// current month.
    /// </summary>
    /// <param name="transactions"></param>
    /// <returns></returns>
    private static List<DateTime>? FindLongestPeriodWithoutSales(List<StockTransaction> transactions, int monthIdx)
    {
        DateOnly firstDateOfMonth = DateOnly.FromDateTime(FirstDateOfMonth(monthIdx));
        DateOnly lastDateOfMonth = DateOnly.FromDateTime(LastDateOfMonth(monthIdx));
        DateOnly? startMaxPeriod = null;
        DateOnly? endMaxPeriod = null;

        if (transactions.Count > 1)
        {

            int i = 0, j = i + 1;

            // compare the date of the first transaction with the start of the month date.
            if (DateOnly.FromDateTime(transactions[0].Date).Day - firstDateOfMonth.Day > 0)
            {
                startMaxPeriod = firstDateOfMonth;
                endMaxPeriod = DateOnly.FromDateTime(transactions[0].Date);
            }

            while (i < transactions.Count && j < transactions.Count)
            {
                DateOnly startPeriod = DateOnly.FromDateTime(transactions[i].Date);
                DateOnly endPeriod = DateOnly.FromDateTime(transactions[j].Date);

                if (startMaxPeriod == null && endMaxPeriod == null)
                {
                    startMaxPeriod = startPeriod;
                    endMaxPeriod = endPeriod;
                }
                else if (endPeriod.Day - startPeriod.Day > endMaxPeriod!.Value.Day - startMaxPeriod!.Value.Day)
                {
                    startMaxPeriod = startPeriod;
                    endMaxPeriod = endPeriod;
                }

                ++i;
                ++j;
            }

            // compare the date of the last transaction and the end of the month date.
            int len = transactions.Count;
            if (lastDateOfMonth.Day - DateOnly.FromDateTime(transactions[len - 1].Date).Day >
                endMaxPeriod!.Value.Day - startMaxPeriod!.Value.Day)
            {
                startMaxPeriod = DateOnly.FromDateTime(transactions[len - 1].Date);
                endMaxPeriod = lastDateOfMonth;
            }

            List<DateTime> period = [
                new DateTime(startMaxPeriod!.Value.Year,startMaxPeriod!.Value.Month, startMaxPeriod!.Value.Day),
                new DateTime(endMaxPeriod!.Value.Year,endMaxPeriod!.Value.Month, endMaxPeriod!.Value.Day),
            ];

            return period;
        }
        else if (transactions.Count == 1)
        {
            // compare the date of the first transaction with the start of the month date.
            if (DateOnly.FromDateTime(transactions[0].Date).Day - firstDateOfMonth.Day >
                lastDateOfMonth.Day - DateOnly.FromDateTime(transactions[0].Date).Day)
            {
                startMaxPeriod = firstDateOfMonth;
                endMaxPeriod = DateOnly.FromDateTime(transactions[0].Date);
            }
            else
            {
                startMaxPeriod = DateOnly.FromDateTime(transactions[0].Date);
                endMaxPeriod = lastDateOfMonth;
            }

            List<DateTime> period = [
                new DateTime(startMaxPeriod!.Value.Year,startMaxPeriod!.Value.Month, startMaxPeriod!.Value.Day),
                new DateTime(endMaxPeriod!.Value.Year,endMaxPeriod!.Value.Month, endMaxPeriod!.Value.Day),
            ];

            return period;
        }

        return null;
    }

    /// <summary>
    /// Given the sales transactions of a product, it determines the longest period with
    /// consecutive sales in the current month.
    /// </summary>
    /// <param name="transactions"></param>
    /// <returns></returns>
    private static List<DateTime>? FindLongestPeriodWithConsecutiveSales(List<StockTransaction> transactions)
    {

        if (transactions.Count > 1)
        {
            int i = 0, j = i + 1, idx = 1;
            List<DateTime> period = [];
            bool isDiscontinued = false;

            while (i < transactions.Count)
            {
                DateOnly startDate = DateOnly.FromDateTime(transactions[i].Date);

                while (j < transactions.Count)
                {
                    DateOnly endDate = DateOnly.FromDateTime(transactions[j].Date);
                    DateOnly nextDate = startDate.AddDays(idx);

                    if (endDate == nextDate)
                    {
                        ++j;
                        ++idx;

                        if (j >= transactions.Count)
                        {
                            period.Clear();
                            period.Add(new DateTime(startDate.Year, startDate.Month, startDate.Day));
                            period.Add(new DateTime(endDate.Year, endDate.Month, endDate.Day));
                            isDiscontinued = true;
                            break;
                        }
                    }
                    else
                    {
                        period.Clear();
                        period.Add(new DateTime(startDate.Year, startDate.Month, startDate.Day));
                        period.Add(transactions[j - 1].Date);
                        isDiscontinued = true;
                        break;
                    }
                }

                if (isDiscontinued)
                    break;

                ++i;
            }

            if (period.Count > 0)
                return period;
        }
        else if (transactions.Count == 1)
        {
            List<DateTime> period = [];
            period.Add(transactions[0].Date);
            period.Add(transactions[0].Date);
            return period;
        }

        return null;
    }

    /// <summary>
    /// Returns the first date of the given month.
    /// </summary>
    /// <returns></returns>
    private static DateTime FirstDateOfMonth(int monthIdx) => new DateTime(DateTime.Now.Year, monthIdx, 1);
        

    /// <summary>
    /// Returns the last date of the given month.
    /// </summary>
    /// <returns></returns>
    private static DateTime LastDateOfMonth(int monthIdx)
    {
        return monthIdx switch
        {
            2 => new DateTime(DateTime.Now.Year, monthIdx, DateTime.Now.Day).AddDays(28 - DateTime.Now.Day)  ,//DateTime.Now.AddDays(28 - DateTime.Now.Day),
            4 or 6 or 9 or 11 => new DateTime(DateTime.Now.Year, monthIdx, DateTime.Now.Day).AddDays(30 - DateTime.Now.Day), //DateTime.Now.AddDays(30 - DateTime.Now.Day),
            1 or 3 or 5 or 7 or 8 or 10 or 12 => new DateTime(DateTime.Now.Year, monthIdx, DateTime.Now.Day).AddDays(31 - DateTime.Now.Day), //,DateTime.Now.AddDays(31 - DateTime.Now.Day),
            _ => DateTime.Now,
        };
    }

    /// <summary>
    /// Compiles a summary of the sales transactions for the given month.
    /// </summary>
    /// <param name="transactions"></param>
    /// <param name="reasonType"></param>
    /// <returns></returns>
    private List<StockTransactionSummary>? CompileSalesStockTransactionsSummaries(List<StockTransaction> transactions, ReasonType reasonType, int monthIdx)
    {

        if (transactions.Count > 0 && reasonType.Reason == "Sold")
        {

            // The transaction summaries for each product.
            List<StockTransactionSummary> summaries = [];

            // How many sales transactions does each product have?
            var numTransactionsForEachProduct =
                from transaction in transactions
                join rType in _context.ReasonTypes on transaction.ReasonTypeId equals rType.Id
                where rType.Reason == "Sold" && transaction.Date.Month == monthIdx
                group transaction by transaction.ProductId into transactionGroup
                select new
                {
                    ProductId = transactionGroup.Key,
                    NumTransactions = transactionGroup.Count()
                };            

            // What is the largest sale for each product in the given month?
            var largestSaleForEachProduct =
                from transaction in transactions
                join product in _context.Products on transaction.ProductId equals product.SKU
                join rType in _context.ReasonTypes on transaction.ReasonTypeId equals rType.Id
                where rType.Reason == "Sold" && transaction.Date.Month == monthIdx
                group transaction by transaction.ProductId into transactionGroup
                select new
                {
                    ProductId = transactionGroup.Key,
                    LargestSale = (
                        from trans in transactionGroup
                        join prod in _context.Products on trans.ProductId equals prod.SKU
                        select trans.QuantityChange * prod.UnitPrice
                    ).Max()
                };

            // What is the lowest sale for each product in the given month?
            var lowestSaleForEachProduct =
                from transaction in transactions
                join product in _context.Products on transaction.ProductId equals product.SKU
                join rType in _context.ReasonTypes on transaction.ReasonTypeId equals rType.Id
                where rType.Reason == "Sold" && transaction.Date.Month == monthIdx
                group transaction by transaction.ProductId into transactionGroup
                select new
                {
                    ProductId = transactionGroup.Key,
                    LowestSale = (
                        from trans in transactionGroup
                        join prod in _context.Products on trans.ProductId equals prod.SKU
                        select trans.QuantityChange * prod.UnitPrice
                    ).Min()
                };

            // What is the total sale for each product in the given month?
            var totalSalesForEachProduct =
                from transaction in transactions
                join product in _context.Products on transaction.ProductId equals product.SKU
                join rType in _context.ReasonTypes on transaction.ReasonTypeId equals rType.Id
                where rType.Reason == "Sold" && transaction.Date.Month == monthIdx
                group transaction by transaction.ProductId into transactionGroup
                select new
                {
                    ProductId = transactionGroup.Key,
                    TotalSales = (
                        from trans in transactionGroup
                        join prod in _context.Products on trans.ProductId equals prod.SKU
                        select trans.QuantityChange * prod.UnitPrice
                    ).Sum()
                };

            // How many units of each product have been sold and how many are unsold as of the given month?
            var quantityUnitsSoldAndUnsoldForeachProduct =
                from trans in transactions
                join rType in _context.ReasonTypes on trans.ReasonTypeId equals rType.Id
                join product in _context.Products on trans.ProductId equals product.SKU
                where rType.Reason == "Sold" && trans.Date.Month == monthIdx
                group trans by new
                {
                    trans.ProductId,
                    product.CurrentStock
                } into transGroup
                select new
                {
                    transGroup.Key.ProductId,

                    QuantityUnitsSold = (from tr in transGroup
                                         select tr.QuantityChange).Sum(),

                    QuantityUnitsUnsold = transGroup.Key.CurrentStock
                };

            // the next index in the transactions list for when creating separate lists.
            int nextIndex = 0;

            // compile the summary
            for (int i = 0; i < numTransactionsForEachProduct.Count(); ++i)
            {
                // create a summary for the product's transactions
                StockTransactionSummary summary = new();
                // stores the longest period without sales for the current product.
                List<DateTime> longestPeriodWithoutSales = [];
                // stores the longest period with consecutive sales for the current product.
                List<DateTime> longestPeriodWithConsecutiveSales = [];
                // contains transactions for a single product.
                List<StockTransaction> stockTransactions = [];

                // copy the transactions for the current product into a new list
                stockTransactions = transactions.Slice(nextIndex, numTransactionsForEachProduct.ElementAt(i).NumTransactions);
                // update next index to refer to the next transaction in the transactions list.
                nextIndex += numTransactionsForEachProduct.ElementAt(i).NumTransactions;
                // find the longest period without sales
                longestPeriodWithoutSales = FindLongestPeriodWithoutSales(stockTransactions, monthIdx)!;
                // find the longest period with consecutive sales
                longestPeriodWithConsecutiveSales = FindLongestPeriodWithConsecutiveSales(stockTransactions)!;

                // compile the summary
                summary.ProductName = _context.Products.FirstOrDefault(p => p.SKU == numTransactionsForEachProduct.ElementAt(i).ProductId)!.Name;                
                summary.StartOfLongestPeriodWithoutSales = longestPeriodWithoutSales.ElementAt(0);
                summary.EndOfLongestPeriodWithoutSales = longestPeriodWithoutSales.ElementAt(1);
                summary.StartOfLongestPeriodWithConsecutiveSales = longestPeriodWithConsecutiveSales.ElementAt(0);
                summary.EndOfLongestPeriodWithConsecutiveSales = longestPeriodWithConsecutiveSales.ElementAt(1);

                for (int k = 0; k < largestSaleForEachProduct.Count(); ++k)
                {
                    if (largestSaleForEachProduct.ElementAt(k).ProductId == numTransactionsForEachProduct.ElementAt(i).ProductId)
                    {
                        summary.LargestSale = largestSaleForEachProduct.ElementAt(k).LargestSale;
                        break;
                    }
                }

                for (int k = 0; k < lowestSaleForEachProduct.Count(); ++k)
                {
                    if (lowestSaleForEachProduct.ElementAt(k).ProductId == numTransactionsForEachProduct.ElementAt(i).ProductId)
                    {
                        summary.LowestSale = lowestSaleForEachProduct.ElementAt(k).LowestSale;
                        break;
                    }
                }

                for (int k = 0; k < totalSalesForEachProduct.Count(); ++k)
                {
                    if (totalSalesForEachProduct.ElementAt(k).ProductId == numTransactionsForEachProduct.ElementAt(i).ProductId)
                    {
                        summary.TotalSales = totalSalesForEachProduct.ElementAt(k).TotalSales;
                        break;
                    }
                }

                for (int k = 0; k < quantityUnitsSoldAndUnsoldForeachProduct.Count(); ++k)
                {
                    if (quantityUnitsSoldAndUnsoldForeachProduct.ElementAt(k).ProductId == numTransactionsForEachProduct.ElementAt(i).ProductId)
                    {
                        summary.QuantityUnitsSold = quantityUnitsSoldAndUnsoldForeachProduct.ElementAt(k).QuantityUnitsSold;
                        summary.QuantityUnitsUnsold = quantityUnitsSoldAndUnsoldForeachProduct.ElementAt(k).QuantityUnitsUnsold;
                        break;
                    }
                }

                // add the summary to the list
                summaries.Add(summary);
            }

            if (summaries.Count > 0)
                return summaries;
        }

        return null;
    }

    /// <summary>
    /// Compiles a summary of the purchases transactions for the current month.
    /// </summary>
    /// <param name="transactions"></param>
    /// <param name="reasonType"></param>
    /// <returns></returns>
    private List<StockTransactionSummary>? CompilePurchasesStockTransactionsSummaries(List<StockTransaction> transactions, ReasonType reasonType, int monthIdx)
    {
        if (transactions.Count > 0 && reasonType.Reason == "Purchased")
        {
            // The transaction summaries for each product.
            List<StockTransactionSummary> summaries = [];

            // How many purchase transactions does each product have?
            var numTransactionsForEachProduct =
                from transaction in transactions
                join rType in _context.ReasonTypes on transaction.ReasonTypeId equals rType.Id
                where rType.Reason == "Purchased" && transaction.Date.Month == monthIdx
                group transaction by transaction.ProductId into transGroup
                select new
                {
                    ProductId = transGroup.Key
                };

            // what is the largest order (purchase) for each product in the given month?
            var largestOrderForeachProduct =
                from transaction in transactions
                join product in _context.Products on transaction.ProductId equals product.SKU
                join rType in _context.ReasonTypes on transaction.ReasonTypeId equals rType.Id
                where rType.Reason == "Purchased" && transaction.Date.Month == monthIdx
                group transaction by transaction.ProductId into transactionGroup
                select new
                {
                    ProductId = transactionGroup.Key,
                    LargestOrder = (from trans in transactionGroup
                                    join pro in _context.Products on trans.ProductId equals pro.SKU
                                    select trans.QuantityChange * pro.CostPrice).Max()
                };

            // What is the lowest order (purchase) for each product in the given month?
            var lowestOrderForeachProduct =
                from transaction in transactions
                join product in _context.Products on transaction.ProductId equals product.SKU
                join rType in _context.ReasonTypes on transaction.ReasonTypeId equals rType.Id
                where rType.Reason == "Purchased" && transaction.Date.Month == monthIdx
                group transaction by transaction.ProductId into transactionGroup
                select new
                {
                    ProductId = transactionGroup.Key,
                    LowestOrder = (from trans in transactionGroup
                                   join product in _context.Products on trans.ProductId equals product.SKU
                                   select trans.QuantityChange * product.CostPrice).Min()
                };

            // How many times has each product been ordered in the given month?
            var NumberOfTimesTheProductHasBeenOrdered =
                from transaction in transactions
                join product in _context.Products on transaction.ProductId equals product.SKU
                join rType in _context.ReasonTypes on transaction.ReasonTypeId equals rType.Id
                where rType.Reason == "Purchased" && transaction.Date.Month == monthIdx
                group transaction by transaction.ProductId into transactionGroup
                select new
                {
                    ProductId = transactionGroup.Key,
                    OrderFrequency =
                        (from trans in transactionGroup
                         select trans).Count()
                };

            // What is the total cost (purchase) for each product in the given month?
            var totalCostForeachProduct =
                from trans in transactions
                join product in _context.Products on trans.ProductId equals product.SKU
                join rType in _context.ReasonTypes on trans.ReasonTypeId equals rType.Id
                where rType.Reason == "Purchased" && trans.Date.Month == monthIdx
                group trans by trans.ProductId into transGroup
                select new
                {
                    ProductId = transGroup.Key,
                    TotalCost = (from trans2 in transGroup
                                 join product in _context.Products on trans2.ProductId equals product.SKU
                                 select trans2.QuantityChange * product.CostPrice).Sum()
                };

            // How many units of each product have been purchased in the given month?
            var quantityUnitsPurchasedForeachProduct =
                from trans in transactions
                join rType in _context.ReasonTypes on trans.ReasonTypeId equals rType.Id
                where rType.Reason == "Purchased" && trans.Date.Month == monthIdx
                group trans by trans.ProductId into transGroup
                select new
                {
                    ProductId = transGroup.Key,
                    QuantityUnitsPurchased = (from tr in transGroup
                                              select tr.QuantityChange).Sum()
                };

            // compile the summary
            for (int i = 0; i < numTransactionsForEachProduct.Count(); ++i)
            {
                // create a summary for the product's transactions
                StockTransactionSummary summary = new()
                {
                    ProductName = _context.Products.FirstOrDefault(p => p.SKU == numTransactionsForEachProduct.ElementAt(i).ProductId)!.Name
                };

                // compile the summary

                for (int k = 0; k < largestOrderForeachProduct.Count(); k++)
                {
                    if (largestOrderForeachProduct.ElementAt(k).ProductId == numTransactionsForEachProduct.ElementAt(i).ProductId)
                    {
                        summary.LargestOrder = largestOrderForeachProduct.ElementAt(k).LargestOrder;
                        break;
                    }
                }

                for (int k = 0; k < lowestOrderForeachProduct.Count(); k++)
                {
                    if (lowestOrderForeachProduct.ElementAt(k).ProductId == numTransactionsForEachProduct.ElementAt(i).ProductId)
                    {
                        summary.LowestOrder = lowestOrderForeachProduct.ElementAt(k).LowestOrder;
                        break;
                    }
                }

                for (int k = 0; k < NumberOfTimesTheProductHasBeenOrdered.Count(); k++)
                {
                    if (NumberOfTimesTheProductHasBeenOrdered.ElementAt(k).ProductId == numTransactionsForEachProduct.ElementAt(i).ProductId)
                    {
                        summary.NumberOfTimesTheProductHasBeenOrdered = NumberOfTimesTheProductHasBeenOrdered.ElementAt(k).OrderFrequency;
                        break;
                    }
                }

                for (int k = 0; k < totalCostForeachProduct.Count(); ++k)
                {
                    if (totalCostForeachProduct.ElementAt(k).ProductId == numTransactionsForEachProduct.ElementAt(i).ProductId)
                    {
                        summary.TotalCost = totalCostForeachProduct.ElementAt(k).TotalCost;
                        break;
                    }
                }

                for (int k = 0; k < quantityUnitsPurchasedForeachProduct.Count(); ++k)
                {
                    if (quantityUnitsPurchasedForeachProduct.ElementAt(k).ProductId == numTransactionsForEachProduct.ElementAt(i).ProductId)
                    {
                        summary.QuantityUnitsPurchased = quantityUnitsPurchasedForeachProduct.ElementAt(k).QuantityUnitsPurchased;
                        break;
                    }
                }

                // add the summary to the list
                summaries.Add(summary);
            }

            if (summaries.Count > 0)
                return summaries;
        }

        return null;
    }

    /// <summary>
    /// Compiles a summary of stock transactions for products that were returned to the supplier this month.
    /// </summary>
    /// <param name="transactions"></param>
    /// <param name="reasonType"></param>
    /// <returns></returns>
    private List<StockTransactionSummary>? CompileReturnsStockTransactions(List<StockTransaction> transactions, ReasonType reasonType, int monthIdx)
    {
        if (transactions.Count > 0 && reasonType.Reason == "Returned")
        {
            // stores transaction summaries for each product.
            List<StockTransactionSummary> summaries = [];

            // How many return transactions does each product have in the given month?
            var numTransactionsForEachProduct =
                from transaction in transactions
                join rType in _context.ReasonTypes on transaction.ReasonTypeId equals rType.Id
                where rType.Reason == "Returned" && transaction.Date.Month == monthIdx
                group transaction by transaction.ProductId into transGroup
                select new
                {
                    ProductId = transGroup.Key
                };

            // How many units of each product have been returned and how much was reclaimed in the given month?
            var quantityUnitsReturnedAndAmountReclaimed =
                from trans in transactions
                join rType in _context.ReasonTypes on trans.ReasonTypeId equals rType.Id
                where rType.Reason == "Returned" && trans.Date.Month == monthIdx
                group trans by trans.ProductId into transGroup
                select new
                {
                    ProductId = transGroup.Key,

                    AmountReclaimed = (from tr in transGroup
                                       join product in _context.Products on tr.ProductId equals product.SKU
                                       select tr.QuantityChange * product.CostPrice).Sum(),

                    QuantityReturned = (from tr in transGroup
                                        select tr.QuantityChange).Sum()
                };

            // compile the summaries

            for (int i = 0; i < numTransactionsForEachProduct.Count(); ++i)
            {
                StockTransactionSummary summary = new()
                {
                    ProductName = _context.Products.FirstOrDefault(p => p.SKU == numTransactionsForEachProduct.ElementAt(i).ProductId)!.Name
                };

                for (int k = 0; k < quantityUnitsReturnedAndAmountReclaimed.Count(); ++k)
                {
                    if (quantityUnitsReturnedAndAmountReclaimed.ElementAt(k).ProductId == numTransactionsForEachProduct.ElementAt(i).ProductId)
                    {
                        summary.AmountReclaimed = quantityUnitsReturnedAndAmountReclaimed.ElementAt(k).AmountReclaimed;
                        summary.QuantityReturned = quantityUnitsReturnedAndAmountReclaimed.ElementAt(k).QuantityReturned;
                        break;
                    }
                }

                // add the summary
                summaries.Add(summary);
            }

            if (summaries.Count > 0)
                return summaries;
        }
        
        return null;
    }

    /// <summary>
    /// Compiles transaction summaries for the products that have been damaged.
    /// </summary>
    /// <param name="transactions"></param>
    /// <param name="reasonType"></param>
    /// <returns></returns>
    private List<StockTransactionSummary>? CompileDamagedStockTransactionSummaries(List<StockTransaction> transactions, ReasonType reasonType, int monthIdx)
    {
        if(transactions.Count > 0 && reasonType.Reason == "Damaged")
        {
            List<StockTransactionSummary> summaries = [];

            var damagedProductsSku =
                from trans in transactions
                join rType in _context.ReasonTypes on trans.ReasonTypeId equals rType.Id
                where rType.Reason == "Damaged" && trans.Date.Month == monthIdx
                group trans by trans.ProductId into transGroup
                select new
                {
                    ProductId = transGroup.Key
                };

            // How many units of each product have been damaged in the given month and what is the total cost for each product?
            var quantityDamagedAndDamageCost =
                from trans in transactions
                join rType in _context.ReasonTypes on trans.ReasonTypeId equals rType.Id
                where rType.Reason == "Damaged" && trans.Date.Month == monthIdx
                group trans by trans.ProductId into transGroup
                select new
                {
                    ProductId = transGroup.Key,

                    QuantityDamaged = (from tr in transGroup
                                       select tr.QuantityChange).Sum(),

                    DamageCost = (from tr in transGroup
                                  join product in _context.Products on tr.ProductId equals product.SKU
                                  select tr.QuantityChange * product.CostPrice).Sum()
                };

            // compile the summary
            for(int i=0; i<damagedProductsSku.Count(); ++i)
            {
                StockTransactionSummary summary = new()
                {
                    ProductName = _context.Products.FirstOrDefault(p => p.SKU == damagedProductsSku.ElementAt(i).ProductId)!.Name
                };

                for (int k = 0; k < quantityDamagedAndDamageCost.Count(); ++k)
                {
                    if (quantityDamagedAndDamageCost.ElementAt(k).ProductId == damagedProductsSku.ElementAt(i).ProductId)
                    {
                        summary.QuantityDamaged = quantityDamagedAndDamageCost.ElementAt(k).QuantityDamaged;
                        summary.DamageCost = quantityDamagedAndDamageCost.ElementAt(k).DamageCost;
                        break;
                    }
                }

                // add the summary
                summaries.Add(summary);
            }

            if (summaries.Count > 0)
                return summaries;
        }

        return null;
    }

    /// <summary>
    /// Generates a summary of the stock transactions for the given month.
    /// </summary>
    /// <param name="reason">Used to specify which category of transaction reasons to
    /// fetch.</param>
    /// <returns></returns>
    public List<StockTransactionSummary>? GetStockTransactionsSummaries(string? reason, int monthIdx)
    {

        if (!string.IsNullOrEmpty(reason) && _context.ReasonTypes.FirstOrDefault(r => r.Reason == reason) is ReasonType rType)
        {
            // Which transactions occurred in the given month?
            List<StockTransaction>? transactions = [..
                from transaction in _context.StockTransactions
                where transaction.ReasonTypeId == rType.Id && transaction.Date.Month == monthIdx
                orderby transaction.ProductId, transaction.Date
                select transaction
            ];

            if (reason == "Sold")
                return CompileSalesStockTransactionsSummaries(transactions, rType, monthIdx);
            else if (reason == "Purchased")
                return CompilePurchasesStockTransactionsSummaries(transactions, rType, monthIdx);
            else if (reason == "Returned")
                return CompileReturnsStockTransactions(transactions, rType, monthIdx);
            else if (reason == "Damaged")
                return CompileDamagedStockTransactionSummaries(transactions, rType, monthIdx);
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
            _context.ReasonTypes.FirstOrDefault(r => r.Reason == reason) is ReasonType reasonType && quantity > 0 && userId >= 0)
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
                && _context.ReasonTypes.FirstOrDefault(r => r.Reason == reason) is ReasonType reasonType && quantity > 0)
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