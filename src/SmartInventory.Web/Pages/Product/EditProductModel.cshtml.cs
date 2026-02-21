using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Product;

public class EditProductModel(HttpClient client) : PageModel
{
    private readonly HttpClient _client = client;

    /// <summary>
    /// Stores the product's data.
    /// </summary>
    [BindProperty]
    public ProductDto? Product { get; set; } = new();

    /// <summary>
    /// Stores the status of the operation.
    /// </summary>
    [BindProperty]
    public string IsEdited { get; set; } = "";

    /// <summary>
    /// Stores a status indicating that product data was fetched.
    /// </summary>
    [BindProperty]
    public string IsFetched { get; set; } = "";

    public void OnGet() { }

    /// <summary>
    /// Fetches a product's data before its being updated.
    /// </summary>
    /// <returns></returns>
    public async Task OnPostFetchData()
    {
        HttpResponseMessage? resp = null;

        // send the request to fetch the data using the sku of the product.
        if (IsValid(Product!.SKU!))
            resp = await _client.GetAsync($"http://192.168.43.172:5196/api/Product/ViewProductDetailsBySku/{Product.SKU}");
        else if (IsValid(Product!.Name))
            // send the request to fetch the data using the name of the product
            resp = await _client.GetAsync($"http://192.168.43.172:5196/api/Product/ViewProductDetailsByName/{Product.Name}");

        if (resp != null && resp.IsSuccessStatusCode)
        {
            // convert the json content to Product model.
            Product = JsonSerializer.Deserialize<ProductDto>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (Product == null || !IsValid(Product.Category!))
            {
                IsFetched = "false";
            }
        }
    }

    /// <summary>
    /// Used to send a HTTP Put request to update a product's data.
    /// </summary>
    /// <returns></returns>
    public async Task OnPutEditProduct()
    {

    }
    
    /// <summary>
    /// Checks if the given string is not null or empty.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static bool IsValid(string str) => !string.IsNullOrEmpty(str);
}