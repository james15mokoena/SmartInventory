using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Stock;

public class ViewStockReportModel(HttpClient client) : PageModel
{
    private readonly HttpClient _client = client;

    /// <summary>
    /// A stock report.
    /// </summary>
    [BindProperty]
    public StockReportDto? StockReport { get; set; } = new();

    /// <summary>
    /// Indicates whether the report is generated.
    /// </summary>
    [BindProperty]
    public string? IsGenerated { get; set; } = "";

    public async Task OnGet()
    {
        string? username = HttpContext.Session.GetString("Username");

        if (!string.IsNullOrEmpty(username))
        {
            string company = "Action Computers";

            HttpResponseMessage resp = await _client.GetAsync($"http://192.168.43.172:5196/api/Stock/GetStockReport/{company}/{username}");

            if (resp.IsSuccessStatusCode)
            {
                IsGenerated = "true";
                StockReport = JsonSerializer.Deserialize<StockReportDto>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
                
            else
                IsGenerated = "false";
        }
    }
}