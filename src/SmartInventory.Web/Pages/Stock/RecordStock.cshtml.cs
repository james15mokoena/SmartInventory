using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Stock;

public class RecordStockModel(HttpClient client, ServerConstants server) : PageModel
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

    public void OnGet() 
    {
        if (HttpContext.Session.GetString("Username") is string username)
            TempData["Username"] = username;    
    }

    public async Task OnPostRecordStock()
    {
        string? username = HttpContext.Session.GetString("Username");

        if (!string.IsNullOrEmpty(username) && Transaction!.TransactionReason != "")
        {
            Transaction!.Username = username;

            // convert to JSON object
            StringContent content = new(JsonSerializer.Serialize(Transaction!), Encoding.UTF8, "application/json");

            HttpResponseMessage? resp = null;

            if (Transaction.TransactionReason == "Issued" || Transaction.TransactionReason == "Damaged" ||
                Transaction.TransactionReason == "Returned")
                resp = await _client.PostAsync($"{server.ApiAddress}/Stock/RecordOutgoingStock", content);
            else if(Transaction.TransactionReason == "Received")
                resp = await _client.PostAsync($"{server.ApiAddress}/Stock/RecordIncomingStock", content);

            if (resp != null && resp.IsSuccessStatusCode)
                IsRecorded = "true";
            else
                IsRecorded = "false";
        }
        else
            IsRecorded = "false";
    }

}