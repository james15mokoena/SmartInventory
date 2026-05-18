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

        if (!string.IsNullOrEmpty(username))
            TempData["Username"] = username;

        if (!string.IsNullOrEmpty(username))
        {
            if ((!string.IsNullOrEmpty(action) && action == "Sales") || string.IsNullOrEmpty(action))
            {
                if (await GenerateSalesReports(username, monthIdx))
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
                await GenerateTopFiveMostSellingCategoriesThisMonthReport(username,monthIdx) &&
                await GenerateBottomFiveLeastSellingCategoriesThisMonthReport(username,monthIdx))
                return true;
        }

        return false;
    }
}
