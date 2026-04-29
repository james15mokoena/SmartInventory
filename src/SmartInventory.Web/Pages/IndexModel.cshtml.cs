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
    /// Indicates if reports have been generated.
    /// </summary>
    [BindProperty]
    public string IsFetched { get; set; } = "";

    public async Task OnGet()
    {
        string? username = HttpContext.Session.GetString("Username");

        if (!string.IsNullOrEmpty(username))
            TempData["Username"] = username;

        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp = await _client.GetAsync($"{_server.ApiAddress}/Home/ViewMonthlyRevenues/{username}");

            if (resp.IsSuccessStatusCode)
            {
                MonthlyRevenues =
                    JsonSerializer.Deserialize<List<MonthlyRevenue>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (MonthlyRevenues != null && MonthlyRevenues.Count > 0)
                {
                    Months.Clear();
                    Revenues.Clear();

                    foreach (MonthlyRevenue mr in MonthlyRevenues)
                    {
                        Months.Add(mr.Month);
                        Revenues.Add(mr.Revenue);
                    }

                }

                IsFetched = "true";
            }
            else
            {
                IsFetched = "false";
            }
        }

    }
}
