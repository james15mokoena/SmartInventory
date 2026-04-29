using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Product;

public class ViewProductsModel(HttpClient client, ServerConstants server) : PageModel
{
    private readonly HttpClient _client = client;

    /// <summary>
    /// Stores the products to be displayed.
    /// </summary>
    [BindProperty]
    public List<ProductDto>? Products { get; set; } = [];

    public async Task OnGet(string? type)
    {
        string? username = HttpContext.Session.GetString("Username");
        if (username != null)
            TempData["Username"] = username;

        HttpResponseMessage? resp = null;

        // send the request to fetch active products.
        if ((!string.IsNullOrEmpty(type) && type == "active") || type == null)
            resp = await _client.GetAsync($"{server.ApiAddress}/Product/ViewActiveProducts/{username}");
        // send the request to fetch deactivated products.
        else if (!string.IsNullOrEmpty(type) && type == "deactivated")
            resp = await _client.GetAsync($"{server.ApiAddress}/Product/ViewDeactivatedProducts/{username}");
        else if (!string.IsNullOrEmpty(type))
            resp = await _client.GetAsync($"{server.ApiAddress}/Product/ViewProductsByCategory/{type}/{username}");

        if (resp != null && resp.IsSuccessStatusCode)
        {
            // get the content
            Products = JsonSerializer.Deserialize<List<ProductDto>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}