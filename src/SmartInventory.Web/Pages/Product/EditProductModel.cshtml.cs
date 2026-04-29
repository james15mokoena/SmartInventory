using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Product;

public class EditProductModel(HttpClient client, ServerConstants server) : PageModel
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

    public void OnGet()
    {
        if (HttpContext.Session.GetString("Username") is string username)
            TempData["Username"] = username;
    }

    /// <summary>
    /// Fetches a product's data before its being updated.
    /// </summary>
    /// <returns></returns>
    public async Task OnPostFetchData()
    {
        HttpResponseMessage? resp = null;

        // send the request to fetch the data using the sku of the product.
        if (IsValid(Product!.SKU!))
            resp = await _client.GetAsync($"{server.ApiAddress}/Product/ViewProductDetailsBySku/{Product.SKU}/{HttpContext.Session.GetString("Username")}");
        else if (IsValid(Product!.Name))
            // send the request to fetch the data using the name of the product
            resp = await _client.GetAsync($"{server.ApiAddress}/Product/ViewProductDetailsByName/{Product.Name}/{HttpContext.Session.GetString("Username")}");

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
        else if(resp != null)
            IsFetched = "unauthorised";
    }

    /// <summary>
    /// Used to send a HTTP Put request to update a product's data.
    /// </summary>
    /// <returns></returns>
    public async Task OnPostEditProduct()
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
            HttpResponseMessage resp = await _client.PutAsync($"{server.ApiAddress}/Product/EditProduct/{username}", content);

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