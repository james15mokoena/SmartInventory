using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Stock;

public class RecordStockModel(HttpClient client) : PageModel
{
    private readonly HttpClient _client = client;

    /// <summary>
    /// Represents a transaction such as recording incoming stock, outgoing stock or stock adjustment.
    /// </summary>
    [BindProperty]
    public RecordStockDto? Transaction { get; set; } = new();

    /// <summary>
    /// Indicates if incoming stock was recorded successully.
    /// </summary>
    [BindProperty]
    public string? IsRecorded { get; set; } = "";

    public void OnGet() { }

    public async Task OnPostRecordStock()
    {
        string? username = HttpContext.Session.GetString("Username");

        if (!string.IsNullOrEmpty(username) && Transaction!.TransactionReason != "")
        {
            Transaction!.Username = username;

            // convert to JSON object
            StringContent content = new(JsonSerializer.Serialize(Transaction!), Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await _client.PostAsync($"http://192.168.43.172:5196/api/Stock/RecordIncomingStock", content);

            if (resp.IsSuccessStatusCode)
                IsRecorded = "true";
            else
                IsRecorded = "false";
        }
    }

}