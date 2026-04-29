using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Product;

public class ProductDetailsModel(HttpClient client, ServerConstants server) : PageModel
{
    private readonly HttpClient _client = client;

    /// <summary>
    /// Stores the product whose details are to be displayed.
    /// </summary>
    [BindProperty]
    public ProductDto? Product { get; set; } = new();

    /// <summary>
    /// Stores the status of the operation.
    /// </summary>
    [BindProperty]
    public string IsEdited { get; set; } = "";

    public async Task OnGet(string sku)
    {
        if (HttpContext.Session.GetString("Username") is string username)
            TempData["Username"] = username;

        // send the request to fetch the data using the sku of the product.
        HttpResponseMessage resp = await _client.GetAsync(
            $"{server.ApiAddress}/Product/ViewProductDetailsBySku/{sku}/{HttpContext.Session.GetString("Username")}");

        if (resp != null && resp.IsSuccessStatusCode)
        {
            // convert the json content to Product model.
            Product = JsonSerializer.Deserialize<ProductDto>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }

    /// <summary>
    /// Used to send a HTTP Put request to activate or deactivate a product.
    /// </summary>
    /// <returns></returns>
    public async Task OnPostToggleActivation()
    {
        Product!.ImageUrl = Product.ImageUrl ?? "";
        Product!.Barcode = Product.Barcode ?? "";
        
        if (IsValid(Product!.SKU!) && IsValid(Product.Name) && IsValid(Product.Description!) &&
            IsValid(Product.Category!) && Product.UnitPrice >= 0 && Product.CostPrice >= 0 &&
            Product.CurrentStock >= 0 && Product.MinimumStockLevel >= 0 && Product.MaximumStockLevel >= 0 &&
            Product.ReorderQuantity >= 0 && Product.UnitMeasurement != null && Product.SupplierId >= 0 &&
            Product.Barcode != null && Product.ImageUrl != null)
        {
            // convert the model to the json.
            StringContent content = new(JsonSerializer.Serialize(Product), Encoding.UTF8, "application/json");

            string? username = HttpContext.Session.GetString("Username");

            // send the request
            HttpResponseMessage resp = await _client.PutAsync(
                $"{server.ApiAddress}/Product/ActivateOrDeactivateProduct/{Product.SKU}/{username}", content);

            if (resp.IsSuccessStatusCode)
                IsEdited = "true";
            else
                IsEdited = "false";
        }
    }
    
    /// <summary>
    /// Checks if the given string is not null or empty.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static bool IsValid(string str) => !string.IsNullOrEmpty(str);
}