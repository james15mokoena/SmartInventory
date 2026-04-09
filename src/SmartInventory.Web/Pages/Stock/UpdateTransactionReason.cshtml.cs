using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Stock;

public class UpdateTransactionReason(HttpClient client, ServerConstants server) : PageModel
{
    private readonly HttpClient _client = client;

    /// <summary>
    /// Used to change the activation status of a transaction reason.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnGet(int reasonId)
    {
        string? username = HttpContext.Session.GetString("Username");

        HttpResponseMessage resp = await _client.PutAsync($"{server.ApiAddress}/Stock/ToggleTransactionReasonStatus/{reasonId}/{username}", null);

        if (resp.IsSuccessStatusCode)
        {
            return RedirectToPage("./TransactionReason","true");
        }

        return RedirectToPage("./TransactionReason", null);
    }

}