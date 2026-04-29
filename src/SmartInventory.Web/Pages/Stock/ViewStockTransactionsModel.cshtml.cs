using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Stock;

public class ViewStockTransactionsModel(HttpClient client, ServerConstants server) : PageModel
{
    private readonly HttpClient _client = client;

    [BindProperty]
    public string? Reason { get; set; } = "";

    /// <summary>
    /// A summary of all stock transactions.
    /// </summary>
    [BindProperty]
    public List<StockTransactionSummary>? TransactionSummaries { get; set; } = [];

    /// <summary>
    /// Gets the transaction summaries categorised by the specified reason.
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    public async Task GetStockTransactionSummaries(string reason)
    {
        if (!string.IsNullOrEmpty(reason))
        {
            string? username = HttpContext.Session.GetString("Username");

            if (!string.IsNullOrEmpty(username))
            {

                HttpResponseMessage resp =
                    await _client.GetAsync($"{server.ApiAddress}/Stock/ViewStockTransactionSummaries/{username}/{reason}");

                if (resp.IsSuccessStatusCode)
                {
                    TransactionSummaries = JsonSerializer.Deserialize<List<StockTransactionSummary>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    Reason = reason;
                }
            }
        }
    }

    /// <summary>
    /// Used to fetch stock transactions grouped by the specified reason.
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    public async Task OnGet(string? reason = "Sold")
    {
        if (HttpContext.Session.GetString("Username") is string username)
            TempData["Username"] = username;

        if (!string.IsNullOrEmpty(reason))
            await GetStockTransactionSummaries(reason);
        else
            await GetStockTransactionSummaries("Sold");
    }
}