using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Stock;

public class TransactionReasonModel(HttpClient client) : PageModel
{
    private readonly HttpClient _client = client;

    /// <summary>
    /// Stores a status indicating that the add operation was successful or not.
    /// </summary>
    [BindProperty]
    public string IsAdded { get; set; } = "";

    /// <summary>
    /// Stores the transaction reason's data.
    /// </summary>
    [BindProperty]
    public TransactionReasonDto? TransactionReason { get; set; } = new();

    public void OnGet() { }
    
    public async Task OnPostAddReason()
    {
        // get the username
        string? username = HttpContext.Session.GetString("Username");

        // serialize the content
        StringContent content = new(JsonSerializer.Serialize(TransactionReason), Encoding.UTF8, "application/json");

        // send the request
        HttpResponseMessage? resp = await _client.PostAsync($"http://localhost:5196/api/Stock/AddTransactionReason/{username}", content);

        if (resp.IsSuccessStatusCode)
            IsAdded = "true";
        else
            IsAdded = "false";
    }
}