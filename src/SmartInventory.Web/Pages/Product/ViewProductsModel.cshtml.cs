using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Product;

public class ViewProductsModel(HttpClient client) : PageModel
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

        HttpResponseMessage? resp = null;

        // send the request to fetch active products.
        if ((!string.IsNullOrEmpty(type) && type == "active") || type == null)
            resp = await _client.GetAsync($"http://192.168.43.172:5196/api/Product/ViewActiveProducts/{username}");
        // send the request to fetch deactivated products.
        else if (!string.IsNullOrEmpty(type) && type == "deactivated")
            resp = await _client.GetAsync($"http://192.168.43.172:5196/api/Product/ViewDeactivatedProducts/{username}");

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