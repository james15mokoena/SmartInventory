using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Stock;

public class ViewStockTransactionsModel(HttpClient client) : PageModel
{
    private readonly HttpClient _client = client;

    [BindProperty]
    public string? IsFetched { get; set; } = "";

    /// <summary>
    /// Stores stock transactions to be displayed.
    /// </summary>
    [BindProperty]
    public List<StockTransactionDto>? StockTransactions { get; set; } = [];

    public async Task OnGet()
    {
        string? username = HttpContext.Session.GetString("Username");

        if (!string.IsNullOrEmpty(username))
        {
            HttpResponseMessage resp = await _client.GetAsync($"http://192.168.43.172:5196/api/Stock/ViewStockTransactions/{username}");

            if (resp.IsSuccessStatusCode)
            {
                StockTransactions = JsonSerializer.Deserialize<List<StockTransactionDto>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            else
                IsFetched = "false";
        }
    }
}