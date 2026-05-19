using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Login;

public class LoginModel(HttpClient client, ServerConstants server) : PageModel
{
    /// <summary>
    /// Will be used to send HTTP requests to the API.
    /// </summary>
    private readonly HttpClient _client = client;

    /// <summary>
    /// Contains login details.
    /// </summary>
    [BindProperty]
    public LoginDto Details { get; set; } = new();

    /// <summary>
    /// Stores the status from the login process.
    /// </summary>
    public string IsLoggedIn { get; set; } = "";
    
    public void OnGet() { }
    
    /// <summary>
    /// Verifies the login details.
    /// </summary>
    public async Task<IActionResult> OnPostLogin()
    {

        if (string.IsNullOrEmpty(Details.Username) || string.IsNullOrEmpty(Details.Password))
        {
            IsLoggedIn = "false";
            return Page();
        }

        // prepare the request content
        StringContent content = new(JsonSerializer.Serialize(Details), Encoding.UTF8, "application/json");
        
        HttpResponseMessage resp = await _client.PostAsync($"{server.ApiAddress}/User/Login", content);

        if (resp.IsSuccessStatusCode)
        {
            // read the response content
            RoleDto? role = JsonSerializer.Deserialize<RoleDto>(
                await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            HttpContext.Session.SetString("RoleName", role!.Name!);
            HttpContext.Session.SetString("Username", Details.Username);
            IsLoggedIn = "true";
            TempData["Username"] = Details.Username;
            TempData["RoleName"] = role.Name;
            AppContext.SetData("Username", Details.Username);
            AppContext.SetData("RoleName", role.Name);

            // go to the home page
            return RedirectToPage("../Index");
        }
        else
            IsLoggedIn = "false";

        return Page();
    }

}