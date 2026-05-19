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
    public async Task GetStockTransactionSummaries(string reason, int monthIdx)
    {
        if (!string.IsNullOrEmpty(reason))
        {
            string? username = HttpContext.Session.GetString("Username");
            string? acUsername = AppContext.GetData("Username") as string ?? null;

            if (!string.IsNullOrEmpty(username) || acUsername != null)
            {
                HttpContext.Session.SetString("Username", acUsername!);
                TempData["Username"] = acUsername;
                username = acUsername;

                HttpResponseMessage resp =
                    await _client.GetAsync($"{server.ApiAddress}/Stock/ViewStockTransactionSummaries/{username}/{reason}/{monthIdx}");

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
    public async Task OnGet(string? reason = "Sold", int monthIdx = 1)
    {
        if (HttpContext.Session.GetString("Username") is string username)
            TempData["Username"] = username;

        if (!string.IsNullOrEmpty(reason))
            await GetStockTransactionSummaries(reason, monthIdx);
        else
            await GetStockTransactionSummaries("Sold", monthIdx);
    }
}