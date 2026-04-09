using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Product;

public class AddProductModel(HttpClient client, ServerConstants server) : PageModel
{
    /// <summary>
    /// Used to interact with the API.
    /// </summary>
    private readonly HttpClient _client = client;

    /// <summary>
    /// The product to be added.
    /// </summary>
    [BindProperty]
    public ProductDto Product { get; set; } = new();

    /// <summary>
    /// Stores a status indicating whether the product was added or not.
    /// </summary>
    [BindProperty]
    public string IsAdded { get; set; } = "";

    public void OnGet()
    {

    }

    /// <summary>
    /// Adds a new product.
    /// </summary>
    public async Task OnPostAddProduct()
    {
        Product!.DateCreated = DateTime.UtcNow.Date;
        Product.LastUpdated = DateTime.UtcNow.Date;
        Product.ImageUrl = Product.ImageUrl ?? "";
        Product.Barcode = Product.Barcode ?? "";

        if (IsValid(Product.SKU!) && IsValid(Product.Name) && IsValid(Product.Description!) &&
            IsValid(Product.Category!) && Product.UnitPrice >= 0 && Product.CostPrice >= 0 &&
            Product.CurrentStock >= 0 && Product.MinimumStockLevel >= 0 && Product.MaximumStockLevel >= 0 &&
            Product.ReorderQuantity >= 0 && Product.UnitMeasurement != null && Product.SupplierId >= 0 &&
            Product.Barcode != null && Product.ImageUrl != null)
        {
            // convert the product to json
            StringContent content = new(JsonSerializer.Serialize(Product), Encoding.UTF8, "application/json");

            // username of an authorised logged in user.
            string username = HttpContext.Session.GetString("Username")!;

            // send the POST request
            HttpResponseMessage resp = await _client.PostAsync(
                                        $"{server.ApiAddress}/Product/AddProduct/{username}", content);

            if (resp.IsSuccessStatusCode)
                IsAdded = "true";
            else
                IsAdded = "false";
        }
    }

    /// <summary>
    /// Checks if the given string is not null or empty.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static bool IsValid(string str) => !string.IsNullOrEmpty(str);
}