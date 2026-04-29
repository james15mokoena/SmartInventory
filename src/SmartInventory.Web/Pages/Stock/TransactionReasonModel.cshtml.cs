using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Stock;

public class TransactionReasonModel(HttpClient client, ServerConstants server) : PageModel
{
    private readonly HttpClient _client = client;

    /// <summary>
    /// Stores a status indicating that the add operation was successful or not.
    /// </summary>
    [BindProperty]
    public string IsAdded { get; set; } = "";

    /// <summary>
    /// Stores a status indicating that the transaction reasons were fetched or not.
    /// </summary>
    [BindProperty]
    public string IsFetched { get; set; } = "";

    /// <summary>
    /// Stores a status indicating that the transaction reason has been activated / deactivated.
    /// </summary>
    [BindProperty]
    public string IsUpdated { get; set; } = "";

    /// <summary>
    /// Stores the transaction reason's data.
    /// </summary>
    [BindProperty]
    public TransactionReasonDto? TransactionReason { get; set; } = new();

    /// <summary>
    /// Stores reasons.
    /// </summary>
    [BindProperty]
    public List<TransactionReasonDto>? TransactionReasons { get; set; } = [];

    public async Task OnGet(string? isUpdated)
    {
        // is used in the case where the user change the active status of a transaction reason.
        if (!string.IsNullOrEmpty(isUpdated) && isUpdated == "true")
        {
            IsUpdated = "true";
        }

        string? username = HttpContext.Session.GetString("Username");
        if (username != null)
            TempData["Username"] = username;

        // get transaction reasons
        HttpResponseMessage resp = await _client.GetAsync($"{server.ApiAddress}/Stock/ViewTransactionReasons/{username}");

        if (resp.IsSuccessStatusCode)
        {
            TransactionReasons = JsonSerializer.Deserialize<List<TransactionReasonDto>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (TransactionReasons == null || TransactionReasons.Count <= 0)
                IsFetched = "false";
            else
                IsFetched = "true";
        }
    }
    
    public async Task OnPostAddReason()
    {
        // get the username
        string? username = HttpContext.Session.GetString("Username");

        // serialize the content
        StringContent content = new(JsonSerializer.Serialize(TransactionReason), Encoding.UTF8, "application/json");

        // send the request
        HttpResponseMessage? resp = await _client.PostAsync($"{server.ApiAddress}/Stock/AddTransactionReason/{username}", content);

        if (resp.IsSuccessStatusCode)
            IsAdded = "true";
        else
            IsAdded = "false";
    }
}