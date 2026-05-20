using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages;

public class IndexModel(HttpClient client, ServerConstants server) : PageModel
{
    private readonly HttpClient _client = client;

    private readonly ServerConstants _server = server;

    /// <summary>
    /// Stores the monthly revenues.
    /// </summary>
    [BindProperty]
    public List<MonthlyRevenue>? MonthlyRevenues { get; set; } = [];

    /// <summary>
    /// Stores the months with revenues.
    /// </summary>
    [BindProperty]
    public List<string> Months { get; set; } = [];

    /// <summary>
    /// Stores the revenues for each month.
    /// </summary>
    [BindProperty]
    public List<double> Revenues { get; set; } = [];

    /// <summary>
    /// Stores the top 5 most selling categories.
    /// </summary>
    [BindProperty]
    public List<CategorySales>? TopFiveMostSellingCategories { get; set; } = [];

        /// <summary>
    /// Stores the top 5 categories.
    /// </summary>
    [BindProperty]
    public List<string> TopFiveCategories { get; set; } = [];

    /// <summary>
    /// Stores the top 5 categories' sales.
    /// </summary>
    [BindProperty]
    public List<double> TopFiveCategoriesSales { get; set; } = [];

    /// <summary>
    /// Stores the bottom 5 least selling categories.
    /// </summary>
    [BindProperty]
    public List<CategorySales>? BottomFiveLeastSellingCategories { get; set; } = [];

    /// <summary>
    /// Stores the bottom 5 categories.
    /// </summary>
    [BindProperty]
    public List<string> BottomFiveCategories { get; set; } = [];

    /// <summary>
    /// Stores the bottom 5 categories' sales.
    /// </summary>
    [BindProperty]
    public List<double> BottomFiveCategoriesSales { get; set; } = [];

    /// <summary>
    /// Stores categories contributing more than 50% towards the month's revenue and their
    /// contribution is percentage.
    /// </summary>
    public List<CategorySales>? CategoriesContributingMoreThan50Percent { get; set; } = [];

    /// <summary>
    /// Stores the names of categories contributing more than 50% towards the month's revenue.
    /// </summary>
    public List<string> CategoriesCMT50Percent { get; set; } = [];

    /// <summary>
    /// Stores the percentages (contributions) of categories contributing more than 50% towards
    /// the month's revenue.
    /// </summary>
    public List<double> ContributionsMT50Percent { get; set; } = [];

    /// <summary>
    /// Stores categories contributing at most 50% towards the month's revenue and their
    /// contribution is percentage.
    /// </summary>
    public List<CategorySales>? CategoriesContributingAtMost50Percent { get; set; } = [];

    /// <summary>
    /// Stores the names of categories contributing at most 50% towards the month's revenue.
    /// </summary>
    public List<string> CategoriesCAM50Percent { get; set; } = [];

    /// <summary>
    /// Stores the percentages (contributions) of categories contributing at most 50% towards
    /// the month's revenue.
    /// </summary>
    public List<double> ContributionsAM50Percent { get; set; } = [];

    /// <summary>
    /// Stores the top five most selling products in the specified month.
    /// </summary>
    public List<CategorySales>? TopFiveMostSellingProductsInMonth { get; set; } = [];

    /// <summary>
    /// Stores the names of the top five most selling products in the specified month.
    /// </summary>
    public List<string> TopFiveMostSellingProductNames { get; set; } = [];

    /// <summary>
    /// Stores the names of the top five most selling products in the specified month.
    /// </summary>
    public List<double> TopFiveMostSellingProductSales { get; set; } = [];

    /// <summary>
    /// Stores the five least selling products in the specified month.
    /// </summary>
    public List<CategorySales>? FiveLeastSellingProductsInMonth { get; set; } = [];

    /// <summary>
    /// Stores the names of the five least selling products in the specified month.
    /// </summary>
    public List<string> FiveLeastSellingProductNames { get; set; } = [];

    /// <summary>
    /// Stores the sales of the five least selling products in the specified month.
    /// </summary>
    public List<double> FiveLeastSellingProductSales { get; set; } = [];

    /// <summary>
    /// Stores the monthly total costs.
    /// </summary>
    [BindProperty]
    public List<MonthlyRevenue>? MonthlyTotalCosts { get; set; } = [];

    /// <summary>
    /// Stores the months with total costs.
    /// </summary>
    [BindProperty]
    public List<string> MonthsTC { get; set; } = [];

    /// <summary>
    /// Stores the total cost for each month.
    /// </summary>
    [BindProperty]
    public List<double> TotalCosts { get; set; } = [];

    /// <summary>
    /// Stores the top 5 categories with high purchase costs.
    /// </summary>
    [BindProperty]
    public List<CategorySales>? TopFiveCategoriesWithHighPurchaseCosts{ get; set; } = [];

    /// <summary>
    /// Stores the names of top 5 categories with high purchase costs.
    /// </summary>
    [BindProperty]
    public List<string> TopFiveCategoriesWithHighPurchaseCostsNames { get; set; } = [];

    /// <summary>
    /// Stores the costs of the top 5 categories with high purchase costs.
    /// </summary>
    [BindProperty]
    public List<double> TopFiveCategoriesWithHighPurchaseCostsTC { get; set; } = [];

    /// <summary>
    /// Stores the 5 least categories with low purchase costs.
    /// </summary>
    [BindProperty]
    public List<CategorySales>? FiveLeastCategoriesWithLowPurchaseCosts{ get; set; } = [];

    /// <summary>
    /// Stores the names of the five least categories with low purchase costs.
    /// </summary>
    [BindProperty]
    public List<string> FiveLeastCategoriesWithLowPurchaseCostsNames { get; set; } = [];

    /// <summary>
    /// Stores the costs of the five least categories with low purchase costs.
    /// </summary>
    [BindProperty]
    public List<double> FiveLeastCategoriesWithLowPurchaseCostsTC { get; set; } = [];

    /// <summary>
    /// Indicates if reports have been generated.
    /// </summary>
    [BindProperty]
    public string IsFetched { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="action">Indicates whether to generates sales, purchases, returns or damages reports</param>
    /// <returns></returns>
    public async Task OnGet(string? action = "Sales", int monthIdx = 1)
    {
        string? username = HttpContext.Session.GetString("Username");
        string? acUsername = AppContext.GetData("Username") as string ?? null;

        if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(acUsername))
        {
            HttpContext.Session.SetString("Username", acUsername!);
            HttpContext.Session.SetString("RoleName", (AppContext.GetData("RoleName") as string)!);
            TempData["Username"] = acUsername;
            username = acUsername;
        }   

        if (!string.IsNullOrEmpty(username))
        {
            if ((!string.IsNullOrEmpty(action) && action == "Sales") || string.IsNullOrEmpty(action))
            {
                if (await GenerateSalesReports(username, monthIdx))
                    IsFetched = "true";
                else
                    IsFetched = "false";
            }
            else if(!string.IsNullOrEmpty(action) && action == "Purchases")
            {
                if (await GeneratePurchasesReports(username, monthIdx))
                    IsFetched = "true";
                else
                    IsFetched = "false";
            }
            else
                IsFetched = "false";
        }
    }

    /// <summary>
    /// Fetches data for the monthly revenues report.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<bool> GenerateMonthlyRevenuesReport(string username)
    {
        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp = await _client.GetAsync($"{_server.ApiAddress}/Home/ViewMonthlyRevenues/{username}");

            if (resp.IsSuccessStatusCode)
            {
                MonthlyRevenues = JsonSerializer.Deserialize<List<MonthlyRevenue>>(await resp.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (MonthlyRevenues != null && MonthlyRevenues.Count >= 0)
                {
                    Months.Clear();
                    Revenues.Clear();

                    foreach (MonthlyRevenue mr in MonthlyRevenues)
                    {
                        Months.Add(mr.Month);
                        Revenues.Add(mr.Revenue);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Fetches data for the top five most selling categories in the given month report.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<bool> GenerateTopFiveMostSellingCategoriesThisMonthReport(string username, int monthIdx)
    {
        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp = await _client.GetAsync($"{_server.ApiAddress}/Home/GetTopFiveMostSellingCategoriesThisMonth/{username}/{monthIdx}");

            if (resp.IsSuccessStatusCode)
            {
                TopFiveMostSellingCategories = JsonSerializer.Deserialize<List<CategorySales>>(await resp.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (TopFiveMostSellingCategories != null && TopFiveMostSellingCategories.Count >= 0)
                {
                    TopFiveCategories.Clear();
                    TopFiveCategoriesSales.Clear();

                    foreach (var item in TopFiveMostSellingCategories)
                    {
                        TopFiveCategories.Add(item.Category);
                        TopFiveCategoriesSales.Add(item.Sales);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Fetches data for the bottom five least selling categories in the given month report.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<bool> GenerateBottomFiveLeastSellingCategoriesThisMonthReport(string username, int monthIdx)
    {
        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp = await _client.GetAsync($"{_server.ApiAddress}/Home/GetBottomFiveLeastSellingCategoriesThisMonth/{username}/{monthIdx}");

            if (resp.IsSuccessStatusCode)
            {
                BottomFiveLeastSellingCategories = JsonSerializer.Deserialize<List<CategorySales>>(await resp.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (BottomFiveLeastSellingCategories != null && BottomFiveLeastSellingCategories.Count >= 0)
                {
                    BottomFiveCategories.Clear();
                    BottomFiveCategoriesSales.Clear();

                    foreach (var item in BottomFiveLeastSellingCategories)
                    {
                        BottomFiveCategories.Add(item.Category);
                        BottomFiveCategoriesSales.Add(item.Sales);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Generates a chart showing the top five most selling products in the given month.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="monthIdx"></param>
    /// <returns></returns>
    private async Task<bool> GenerateTopFiveMostSellingProductsInMonth(string username, int monthIdx)
    {
        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp = await _client.GetAsync($"{_server.ApiAddress}/Home/GetTopFiveMostSellingProductsInMonth/{username}/{monthIdx}");

            if (resp.IsSuccessStatusCode)
            {
                TopFiveMostSellingProductsInMonth = JsonSerializer.Deserialize<List<CategorySales>>(await resp.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (TopFiveMostSellingProductsInMonth != null && TopFiveMostSellingProductsInMonth.Count >= 0)
                {
                    TopFiveMostSellingProductNames.Clear();
                    TopFiveMostSellingProductSales.Clear();

                    foreach (var item in TopFiveMostSellingProductsInMonth)
                    {
                        TopFiveMostSellingProductNames.Add(item.Category);
                        TopFiveMostSellingProductSales.Add(item.Sales);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Generates a chart showing the five least selling products in the given month.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="monthIdx"></param>
    /// <returns></returns>
    private async Task<bool> GenerateFiveLeastSellingProductsInMonth(string username, int monthIdx)
    {
        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp = await _client.GetAsync($"{_server.ApiAddress}/Home/GetFiveLeastSellingProductsInMonth/{username}/{monthIdx}");

            if (resp.IsSuccessStatusCode)
            {
                FiveLeastSellingProductsInMonth = JsonSerializer.Deserialize<List<CategorySales>>(await resp.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (FiveLeastSellingProductsInMonth != null && FiveLeastSellingProductsInMonth.Count >= 0)
                {
                    FiveLeastSellingProductNames.Clear();
                    FiveLeastSellingProductSales.Clear();

                    foreach (var item in FiveLeastSellingProductsInMonth)
                    {
                        FiveLeastSellingProductNames.Add(item.Category);
                        FiveLeastSellingProductSales.Add(item.Sales);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Fetches data for categories contributing more than 50% towards the given month's revenue.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<bool> GenerateCategoriesContributingMoreThan50PercentMonthReport(string username, int monthIdx)
    {
        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp =
            await _client.GetAsync($"{_server.ApiAddress}/Home/GetCategoriesContributingMoreThan50PercentMonth/{username}/{monthIdx}");

            if (resp.IsSuccessStatusCode)
            {
                CategoriesContributingMoreThan50Percent =
                    JsonSerializer.Deserialize<List<CategorySales>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (CategoriesContributingMoreThan50Percent != null && CategoriesContributingMoreThan50Percent.Count >= 0)
                {
                    CategoriesCMT50Percent.Clear();
                    ContributionsMT50Percent.Clear();

                    foreach (var item in CategoriesContributingMoreThan50Percent)
                    {
                        CategoriesCMT50Percent.Add(item.Category);
                        ContributionsMT50Percent.Add(item.Sales);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Fetches data for categories contributing at most 50% towards the given month's revenue.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<bool> GenerateCategoriesContributingAtMost50PercentMonth(string username, int monthIdx)
    {
        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp =
                await _client.GetAsync($"{_server.ApiAddress}/Home/GetCategoriesContributingAtMost50PercentMonth/{username}/{monthIdx}");

            if (resp.IsSuccessStatusCode)
            {
                CategoriesContributingAtMost50Percent =
                    JsonSerializer.Deserialize<List<CategorySales>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (CategoriesContributingAtMost50Percent != null && CategoriesContributingAtMost50Percent.Count >= 0)
                {
                    CategoriesCAM50Percent.Clear();
                    ContributionsAM50Percent.Clear();

                    foreach (var i in CategoriesContributingAtMost50Percent)
                    {
                        CategoriesCAM50Percent.Add(i.Category);
                        ContributionsAM50Percent.Add(i.Sales);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Fetches data for the top five categories with high purchase costs in the given month report.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<bool> GenerateTopFiveCategoriesWithHighPurchaseCostsMonthReport(string username, int monthIdx)
    {
        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp = await _client.GetAsync($"{_server.ApiAddress}/Home/GetTopFiveCategoriesWithHigherTotalCostInMonth/{username}/{monthIdx}");

            if (resp.IsSuccessStatusCode)
            {
                TopFiveCategoriesWithHighPurchaseCosts = JsonSerializer.Deserialize<List<CategorySales>>(await resp.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (TopFiveCategoriesWithHighPurchaseCosts != null && TopFiveCategoriesWithHighPurchaseCosts.Count >= 0)
                {
                    TopFiveCategoriesWithHighPurchaseCostsNames.Clear();
                    TopFiveCategoriesWithHighPurchaseCostsTC.Clear();

                    foreach (var item in TopFiveCategoriesWithHighPurchaseCosts)
                    {
                        TopFiveCategoriesWithHighPurchaseCostsNames.Add(item.Category);
                        TopFiveCategoriesWithHighPurchaseCostsTC.Add(item.Sales);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Fetches data for the five least categories with low purchase costs in the given month report.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<bool> GenerateFiveLeastCategoriesWithLowPurchaseCostsMonthReport(string username, int monthIdx)
    {
        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp = await _client.GetAsync($"{_server.ApiAddress}/Home/GetFiveLeastCategoriesWithLowTotalCostInMonth/{username}/{monthIdx}");

            if (resp.IsSuccessStatusCode)
            {
                FiveLeastCategoriesWithLowPurchaseCosts = JsonSerializer.Deserialize<List<CategorySales>>(await resp.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (FiveLeastCategoriesWithLowPurchaseCosts != null && FiveLeastCategoriesWithLowPurchaseCosts.Count >= 0)
                {
                    FiveLeastCategoriesWithLowPurchaseCostsNames.Clear();
                    FiveLeastCategoriesWithLowPurchaseCostsTC.Clear();

                    foreach (var item in FiveLeastCategoriesWithLowPurchaseCosts)
                    {
                        FiveLeastCategoriesWithLowPurchaseCostsNames.Add(item.Category);
                        FiveLeastCategoriesWithLowPurchaseCostsTC.Add(item.Sales);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Fetches data for the monthly total costs report.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<bool> GenerateMonthlyTotalCostsReport(string username)
    {
        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp = await _client.GetAsync($"{_server.ApiAddress}/Home/GetMonthlyTotalCost/{username}");

            if (resp.IsSuccessStatusCode)
            {
                MonthlyTotalCosts = JsonSerializer.Deserialize<List<MonthlyRevenue>>(await resp.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (MonthlyTotalCosts != null && MonthlyTotalCosts.Count >= 0)
                {
                    MonthsTC.Clear();
                    TotalCosts.Clear();

                    foreach (var mr in MonthlyTotalCosts)
                    {
                        MonthsTC.Add(mr.Month);
                        TotalCosts.Add(mr.Revenue);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Generates sales reports.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<bool> GenerateSalesReports(string username, int monthIdx)
    {
        if (!string.IsNullOrEmpty(username))
        {
            if (await GenerateMonthlyRevenuesReport(username) &&
                await GenerateCategoriesContributingMoreThan50PercentMonthReport(username, monthIdx) &&
                await GenerateCategoriesContributingAtMost50PercentMonth(username, monthIdx) &&
                await GenerateTopFiveMostSellingCategoriesThisMonthReport(username, monthIdx) &&
                await GenerateBottomFiveLeastSellingCategoriesThisMonthReport(username, monthIdx) &&
                await GenerateTopFiveMostSellingProductsInMonth(username, monthIdx) &&
                await GenerateFiveLeastSellingProductsInMonth(username, monthIdx))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Generates purchases reports.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<bool> GeneratePurchasesReports(string username, int monthIdx)
    {
        if (!string.IsNullOrEmpty(username))
        {
            if (await GenerateMonthlyTotalCostsReport(username) &&
                await GenerateTopFiveCategoriesWithHighPurchaseCostsMonthReport(username, monthIdx) &&
                await GenerateFiveLeastCategoriesWithLowPurchaseCostsMonthReport(username, monthIdx))
                return true;
        }

        return false;
    }
}
