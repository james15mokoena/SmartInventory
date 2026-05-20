using SmartInventory.API.Data;
using SmartInventory.API.Domain.DTO;

namespace SmartInventory.API.Repositories;

public class HomeRepository(DatabaseContext context)
{
    private readonly DatabaseContext _context = context;

    /// <summary>
    /// Finds the first date of the current month.
    /// </summary>
    /// <returns></returns>
    private static DateTime FirstDateOfCurrentMonth() => new (DateTime.Now.Year, DateTime.Now.Month, 1);

    /// <summary>
    /// Finds the last date of the current month.
    /// </summary>
    /// <returns></returns>
    private static DateTime LastDateOfCurrentMonth()
    {
        return DateTime.Now.Month switch
        {
            2 => DateTime.Now.AddDays(28 - DateTime.Now.Day),
            4 or 6 or 9 or 11 => DateTime.Now.AddDays(30 - DateTime.Now.Day),
            1 or 3 or 5 or 7 or 8 or 10 or 12 => DateTime.Now.AddDays(31 - DateTime.Now.Day),
            _ => DateTime.Now,
        };
    }

    /// <summary>
    /// Returns the name of the month with the given index.
    /// </summary>
    /// <param name="monthIdx"></param>
    /// <returns></returns>
    private static string GetMonth(int monthIdx)
    {
        return monthIdx switch
        {
            1 => "Jan",
            2 => "Feb",
            3 => "Mar",
            4 => "Apr",
            5 => "May",
            6 => "Jun",
            7 => "Jul",
            8 => "Aug",
            9 => "Sep",
            10 => "Oct",
            11 => "Nov",
            12 => "Dec",
            _ => "Jan"
        };
    }

    /// <summary>
    /// Query: <b>What are top 5 most selling products in a given month?</b>
    /// </summary>
    /// <param name="monthIdx"></param>
    /// <returns></returns>
    public List<CategorySales>? GetTopFiveMostSellingProductsInMonth(int monthIdx)
    {
        var topFiveMostSelling =
            (from trans in _context.StockTransactions
             from product in _context.Products
             from rType in _context.ReasonTypes
             where trans.ProductId == product.SKU && trans.ReasonTypeId == rType.Id && trans.Date.Month == monthIdx
                && rType.Reason == "Sold"
             group trans by product.Name into tGroup
             select new
             {
                 Name = tGroup.Key
                 ,
                 Sales = (from trans2 in tGroup
                          join prod2 in _context.Products on trans2.ProductId equals prod2.SKU
                          select trans2.QuantityChange * prod2.UnitPrice).Sum()
             } into res
             orderby res.Sales descending
             select res).Take(5);

        if (topFiveMostSelling.Any())
        {
            List<CategorySales> fiveMostSellingProducts = [];

            foreach (var item in topFiveMostSelling)
            {
                fiveMostSellingProducts.Add(new()
                {
                    Category = item.Name
                    ,
                    Sales = item.Sales
                });
            }

            return fiveMostSellingProducts;
        }

        return null;
    }
    
    /// <summary>
    /// Query: <b>What are 5 least selling products in a given month?</b>
    /// </summary>
    /// <param name="monthIdx"></param>
    /// <returns></returns>
    public List<CategorySales>? GetFiveLeastSellingProductsInMonth(int monthIdx)
    {
        var fiveLeastSelling =
            (from trans in _context.StockTransactions
            from product in _context.Products
            from rType in _context.ReasonTypes
            where trans.ProductId == product.SKU && trans.ReasonTypeId == rType.Id && trans.Date.Month == monthIdx
                && rType.Reason == "Sold"
            group trans by product.Name into tGroup
            select new
            {
                Name = tGroup.Key
                ,
                Sales = (from trans2 in tGroup
                         join prod2 in _context.Products on trans2.ProductId equals prod2.SKU
                         select trans2.QuantityChange * prod2.UnitPrice).Sum()
            } into res
            orderby res.Sales
            select res).Take(5);

        if (fiveLeastSelling.Any())
        {
            List<CategorySales> fiveLeastSellingProducts = [];

            foreach (var item in fiveLeastSelling)
            {
                fiveLeastSellingProducts.Add(new()
                {
                    Category = item.Name
                    ,
                    Sales = item.Sales
                });
            }

            return fiveLeastSellingProducts;
        }

        return null;
    }

    /// <summary>
    /// Query: <b>Which product categories contribute at most 50% of the total revenue for the given month?</b>
    /// </summary>
    /// <returns></returns>
    public List<CategorySales>? GetCategoriesContributingAtMost50PercentMonth(int monthIdx)
    {

        var result =
            from trans in _context.StockTransactions
            join product in _context.Products on trans.ProductId equals product.SKU
            join r in _context.ReasonTypes on trans.ReasonTypeId equals r.Id
            where r.Reason == "Sold" && trans.Date.Month == monthIdx
            group trans by product.Category into tGroup
            select new
            {
                Category = tGroup.Key,
                Percentage =
                    (from t in tGroup
                     join product2 in _context.Products on t.ProductId equals product2.SKU
                     select t.QuantityChange * product2.UnitPrice).Sum()
            } into res
            orderby res.Percentage descending
            select res;

        // compute the total revenue for the given month
        double revenue =
            (from trans in _context.StockTransactions
             join product in _context.Products on trans.ProductId equals product.SKU
             join r in _context.ReasonTypes on trans.ReasonTypeId equals r.Id
             where r.Reason == "Sold" && trans.Date.Month == monthIdx
             select trans.QuantityChange * product.UnitPrice).Sum();

        if (result.Any())
        {
            List<CategorySales> results = [];

            foreach (var item in result)
            {
                CategorySales category = new()
                {
                    Category = item.Category,
                    Sales = 100 * (item.Percentage / revenue)
                };

                if (category.Sales <= 50)
                    results.Add(category);
            }

            return results;
        }

        return null;
    }

    /// <summary>
    /// Query: <b>Which product categories contribute at most 50% of the total revenue for the given month?</b>
    /// </summary>
    /// <returns></returns>
    public List<CategorySales>? GetCategoriesContributingMoreThan50PercentMonth(int monthIdx)
    {

        var result =
            from trans in _context.StockTransactions
            join product in _context.Products on trans.ProductId equals product.SKU
            join r in _context.ReasonTypes on trans.ReasonTypeId equals r.Id
            where r.Reason == "Sold" && trans.Date.Month == monthIdx
            group trans by product.Category into tGroup
            select new
            {
                Category = tGroup.Key,
                Percentage =
                    (from t in tGroup
                     join product2 in _context.Products on t.ProductId equals product2.SKU
                     select t.QuantityChange * product2.UnitPrice).Sum()
            } into res
            orderby res.Percentage descending
            select res;

        // compute the total revenue for the given month
        double revenue =
            (from trans in _context.StockTransactions
             join product in _context.Products on trans.ProductId equals product.SKU
             join r in _context.ReasonTypes on trans.ReasonTypeId equals r.Id
             where r.Reason == "Sold" && trans.Date.Month == monthIdx
             select trans.QuantityChange * product.UnitPrice).Sum();

        if (result.Any())
        {
            List<CategorySales> results = [];

            foreach (var item in result)
            {
                CategorySales category = new()
                {
                    Category = item.Category,
                    Sales = 100 * (item.Percentage / revenue)
                };

                if (category.Sales > 50)
                    results.Add(category);
            }

            return results;
        }

        return null;
    }

    /// <summary>
    /// Answers: <b>Which product categories make up the top 5 most sales in the given month?</b>
    /// </summary>
    /// <returns></returns>
    public List<CategorySales>? GetTopFiveMostSellingCategoriesThisMonth(int monthIdx)
    {
        var topFiveMostSelling =
            (from trans in _context.StockTransactions
             join product in _context.Products on trans.ProductId equals product.SKU
             join rType in _context.ReasonTypes on trans.ReasonTypeId equals rType.Id
             where rType.Reason == "Sold" && trans.Date.Month == monthIdx
             group trans by product.Category into tGroup
             select new
             {
                 Category = tGroup.Key,
                 Sales = (from trans2 in tGroup
                          join product2 in _context.Products on trans2.ProductId equals product2.SKU
                          select trans2.QuantityChange * product2.UnitPrice).Sum()
             } into result
             orderby result.Sales descending
             select result).Take(5);

        if (topFiveMostSelling.Any())
        {
            List<CategorySales> mostSelling = [];

            foreach (var res in topFiveMostSelling)
            {
                CategorySales category = new()
                {
                    Category = res.Category,
                    Sales = res.Sales
                };

                mostSelling.Add(category);
            }

            return mostSelling;
        }

        return null;
    }

    /// <summary>
    /// Answers: <b>Which product categories make up the bottom 5 least sales in the given month?</b>
    /// </summary>
    /// <returns></returns>
    public List<CategorySales>? GetBottomFiveMostLeastCategoriesThisMonth(int monthIdx)
    {
        var bottomFiveLeastSelling =
            (from trans in _context.StockTransactions
             join product in _context.Products on trans.ProductId equals product.SKU
             join rType in _context.ReasonTypes on trans.ReasonTypeId equals rType.Id
             where rType.Reason == "Sold" && trans.Date.Month == monthIdx
             group trans by product.Category into tGroup
             select new
             {
                 Category = tGroup.Key,
                 Sales = (from trans2 in tGroup
                          join product2 in _context.Products on trans2.ProductId equals product2.SKU
                          select trans2.QuantityChange * product2.UnitPrice).Sum()
             } into result
             orderby result.Sales
             select result).Take(5);

        if (bottomFiveLeastSelling.Any())
        {
            List<CategorySales> leastSelling = [];

            foreach (var res in bottomFiveLeastSelling)
            {
                CategorySales category = new()
                {
                    Category = res.Category,
                    Sales = res.Sales
                };

                leastSelling.Add(category);
            }

            return leastSelling;
        }

        return null;
    }

    /// <summary>
    /// Answers: <b>What is the total revenue for each month this year?</b>
    /// </summary>
    /// <returns></returns>
    public List<MonthlyRevenue>? GetMonthlyRevenues()
    {
        // the first and last dates of the current year.
        DateTime startDate = new(DateTime.Now.Year, 1, 1);
        DateTime lastDate = new(DateTime.Now.Year, 12, 31);

        var monthlyRevenues =
            from trans in _context.StockTransactions
            join product in _context.Products on trans.ProductId equals product.SKU
            join rType in _context.ReasonTypes on trans.ReasonTypeId equals rType.Id
            where rType.Reason == "Sold" && trans.Date >= startDate && trans.Date <= lastDate
            group trans by trans.Date.Month into transGroup
            orderby transGroup.Key
            select new
            {
                Month = transGroup.Key,
                Revenue =
                    (from trans2 in transGroup
                     join product2 in _context.Products on trans2.ProductId equals product2.SKU
                     select trans2.QuantityChange * product2.UnitPrice).Sum()
            };

        if (monthlyRevenues.Any())
        {
            List<MonthlyRevenue> revenues = [];
            int i = 0;

            while (i < monthlyRevenues.Count())
            {
                MonthlyRevenue monthlyRevenue = new()
                {
                    Month = GetMonth(monthlyRevenues.ElementAt(i).Month),
                    Revenue = monthlyRevenues.ElementAt(i).Revenue
                };

                revenues.Add(monthlyRevenue);
                ++i;
            }

            return revenues;
        }

        return null;
    }

    /// <summary>
    /// Answers the question: <b>What is the total revenue for this month?</b>
    /// </summary>
    /// <returns></returns>
    public double GetTotalRevenueCurrentMonth()
    {
        double totalRevenue =
            (from trans in _context.StockTransactions
             join product in _context.Products on trans.ProductId equals product.SKU
             join rType in _context.ReasonTypes on trans.ReasonTypeId equals rType.Id
             where rType.Reason == "Sold" && trans.Date >= FirstDateOfCurrentMonth() &&
                 trans.Date <= LastDateOfCurrentMonth()
             select trans.QuantityChange * product.UnitPrice).Sum();
        return totalRevenue;
    }

    /// <summary>
    /// Answers the Question: <b>What is the total sales for each product category for the given month?</b>
    /// </summary>
    /// <returns></returns>
    public List<TotalSalesByCategory>? GetTotalSalesByCategories(int monthIdx)
    {
        var totalSalesByCategory =
            from trans in _context.StockTransactions
            join product in _context.Products on trans.ProductId equals product.SKU
            join rType in _context.ReasonTypes on trans.ReasonTypeId equals rType.Id
            where rType.Reason == "Sold" && trans.Date.Month == monthIdx
            group trans by product.Category into transGroup
            select new
            {
                Category = transGroup.Key,
                TotalSales = (from trans2 in transGroup
                              join prod in _context.Products on trans2.ProductId equals prod.SKU
                              select trans2.QuantityChange * prod.UnitPrice).Sum()
            };

        List<TotalSalesByCategory> totalSalesByCategories = [];

        if (totalSalesByCategory != null && totalSalesByCategory.Any())
        {
            for (int i = 0; i < totalSalesByCategory.Count(); ++i)
            {
                totalSalesByCategories.Add(new()
                {
                    Category = totalSalesByCategory.ElementAt(i).Category
                    ,
                    TotalSales = totalSalesByCategory.ElementAt(i).TotalSales
                });
            }

            return totalSalesByCategories;
        }

        return null;
    }

    /// <summary>
    /// Answers: <b>Which product category has the most sales in the given month?</b>
    /// </summary>
    /// <returns></returns>
    public TotalSalesByCategory? GetCategoryWithMostSales(int monthIdx)
    {
        List<TotalSalesByCategory>? tsbyc = GetTotalSalesByCategories(monthIdx);

        if (tsbyc != null && tsbyc.Count > 0)
            return tsbyc.OrderByDescending(r => r.TotalSales).First();

        return null;
    }

    /// <summary>
    /// Answers: <b>Which product category has the least sales in the given month?</b>
    /// </summary>
    /// <returns></returns>
    public TotalSalesByCategory? GetCategoryWithLeastSales(int monthIdx)
    {
        List<TotalSalesByCategory>? tsbyc = GetTotalSalesByCategories(monthIdx);

        if (tsbyc != null && tsbyc.Count > 0)
            return tsbyc.OrderBy(r => r.TotalSales).First();

        return null;
    }

    /// <summary>
    /// Answers: <b>Which product has the most sales in each category?</b>
    /// </summary>
    /// <returns></returns>
    public List<TotalSalesByCategory>? GetProductsWithMostSalesByCategory(int monthIdx)
    {

        var results =
            from trans in _context.StockTransactions
            join product in _context.Products on trans.ProductId equals product.SKU
            join reason in _context.ReasonTypes on trans.ReasonTypeId equals reason.Id
            where reason.Reason == "Sold" && trans.Date.Month == monthIdx 
            group trans by new
            {
                product.Category
                ,
                ProductName = product.Name
            } into transGroup
            orderby transGroup.Key.Category
            select new
            {
                transGroup.Key.Category,
                transGroup.Key.ProductName,
                TotalSales =
                    (from trans2 in transGroup
                     join prod in _context.Products on trans2.ProductId equals prod.SKU
                     select trans2.QuantityChange * prod.UnitPrice).Sum()
            } into transGroup2
            orderby transGroup2.Category, transGroup2.TotalSales descending
            select transGroup2;

        TotalSalesByCategory? last = null;
        List<TotalSalesByCategory> productsWithMostSalesInTheirCategories = [];

        for (int i = 0; i < results.Count(); ++i)
        {
            TotalSalesByCategory obj = new()
            {
                Category = results.ElementAt(i).Category,
                ProductName = results.ElementAt(i).ProductName,
                TotalSales = results.ElementAt(i).TotalSales
            };

            if (last == null)
            {
                last = obj;
                productsWithMostSalesInTheirCategories.Add(last);
            }
            else if (last.Category != obj.Category)
            {
                last = obj;
                productsWithMostSalesInTheirCategories.Add(last);
            }
        }

        if (productsWithMostSalesInTheirCategories.Count > 0)
            return productsWithMostSalesInTheirCategories;

        return null;
    }

    /// <summary>
    /// Answers: <b>Which product has the least sales in each category?</b>
    /// </summary>
    /// <returns></returns>
    public List<TotalSalesByCategory>? GetProductsWithLeastSalesByCategory(int monthIdx)
    {

        var results =
            from trans in _context.StockTransactions
            join product in _context.Products on trans.ProductId equals product.SKU
            join reason in _context.ReasonTypes on trans.ReasonTypeId equals reason.Id
            where reason.Reason == "Sold" && trans.Date.Month == monthIdx
            group trans by new
            {
                product.Category
                ,
                ProductName = product.Name
            } into transGroup
            orderby transGroup.Key.Category
            select new
            {
                transGroup.Key.Category,
                transGroup.Key.ProductName,
                TotalSales =
                    (from trans2 in transGroup
                     join prod in _context.Products on trans2.ProductId equals prod.SKU
                     select trans2.QuantityChange * prod.UnitPrice).Sum()
            } into transGroup2
            orderby transGroup2.Category, transGroup2.TotalSales ascending
            select transGroup2;

        TotalSalesByCategory? last = null;
        List<TotalSalesByCategory> productsWithLeastSalesInTheirCategories = [];

        for (int i = 0; i < results.Count(); ++i)
        {
            TotalSalesByCategory obj = new()
            {
                Category = results.ElementAt(i).Category,
                ProductName = results.ElementAt(i).ProductName,
                TotalSales = results.ElementAt(i).TotalSales
            };

            if (last == null)
            {
                last = obj;
                productsWithLeastSalesInTheirCategories.Add(last);
            }
            else if (last.Category != obj.Category)
            {
                last = obj;
                productsWithLeastSalesInTheirCategories.Add(last);
            }
        }

        if (productsWithLeastSalesInTheirCategories.Count > 0)
            return productsWithLeastSalesInTheirCategories;

        return null;
    }

    /// <summary>
    /// Query: <b>What is the total cost of purchases for the past months?</b>
    /// </summary>
    /// <returns></returns>
    public List<MonthlyRevenue>? GetMonthlyTotalCost()
    {
        var monthlyTotalCosts =
            from trans in _context.StockTransactions
            from product in _context.Products
            where trans.ProductId == product.SKU
            from rtype in _context.ReasonTypes
            where rtype.Id == trans.ReasonTypeId && rtype.Reason == "Purchased"
            group trans by trans.Date.Month into tg
            select new
            {
                Month = tg.Key,
                TotalCost =
                    (from trans2 in tg
                     from prod in _context.Products
                     where trans2.ProductId == prod.SKU
                     select trans2.QuantityChange * prod.CostPrice).Sum()
            } into res
            orderby res.Month
            select res;

        if (monthlyTotalCosts.Any())
        {
            List<MonthlyRevenue> _monthlyTotalCosts = [];

            foreach (var item in monthlyTotalCosts)
            {
                _monthlyTotalCosts.Add(new()
                {
                    Month = GetMonth(item.Month),
                    Revenue = item.TotalCost
                });
            }

            return _monthlyTotalCosts;
        }

        return null;
    }

    /// <summary>
    /// Query: <b>What are top 5 categories with higher total cost of purchases/inventory in the given month?</b>
    /// </summary>
    /// <param name="monthIdx"></param>
    /// <returns></returns>
    public List<CategorySales>? GetTopFiveCategoriesWithHigherTotalCostInMonth(int monthIdx)
    {
        var query =
            (from trans in _context.StockTransactions
             where trans.Date.Month == monthIdx
             from product in _context.Products
             where trans.ProductId == product.SKU
             from rtype in _context.ReasonTypes
             where rtype.Id == trans.ReasonTypeId && rtype.Reason == "Purchased"
             group trans by product.Category into tg
             select new
             {
                 Category = tg.Key
                 ,
                 TotalCost =
                     (from tr in tg
                      from prod in _context.Products
                      where tr.ProductId == prod.SKU
                      select tr.QuantityChange * prod.CostPrice).Sum()
             } into res
             orderby res.TotalCost descending
             select res).Take(5);

        if (query.Any())
        {
            List<CategorySales> categoryTotalCost = [];

            foreach (var item in query)
            {
                categoryTotalCost.Add(new()
                {
                    Category = item.Category,
                    Sales = item.TotalCost
                });
            }

            return categoryTotalCost;
        }

        return null;
    }
    
    /// <summary>
    /// Query: <b>What are 5 least categories with low total cost of purchases?</b>
    /// </summary>
    /// <param name="monthIdx"></param>
    /// <returns></returns>
    public List<CategorySales>? GetFiveLeastCategoriesWithLowTotalCostInMonth(int monthIdx)
    {
        var query =
            (from trans in _context.StockTransactions
            where trans.Date.Month == monthIdx
            from product in _context.Products
            where trans.ProductId == product.SKU
            from rtype in _context.ReasonTypes
            where rtype.Id == trans.ReasonTypeId && rtype.Reason == "Purchased"
            group trans by product.Category into tg
            select new
            {
                Category = tg.Key
                ,
                TotalCost =
                    (from tr in tg
                     from prod in _context.Products
                     where tr.ProductId == prod.SKU
                     select tr.QuantityChange * prod.CostPrice).Sum()
            } into res
            orderby res.TotalCost
            select res).Take(5);

        if (query.Any())
        {
            List<CategorySales> categoryTotalCost = [];

            foreach (var item in query)
            {
                categoryTotalCost.Add(new()
                {
                    Category = item.Category,
                    Sales = item.TotalCost
                });
            }

            return categoryTotalCost;
        }

        return null;
    }
}