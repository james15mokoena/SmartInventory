using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Login;

public class LoginModel(HttpClient client) : PageModel
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
    public async Task OnPostLogin()
    {

        if (string.IsNullOrEmpty(Details.Username) || string.IsNullOrEmpty(Details.Password))
        {
            IsLoggedIn = "False";
            Details.Username = "";
            Details.Password = "";
            return;
        }

        StringContent content = new(JsonSerializer.Serialize(Details), Encoding.UTF8, "application/json");

        HttpResponseMessage resp = await _client.PostAsync("http://192.168.43.172:5196/api/User/Login", content);

        if (resp.IsSuccessStatusCode)
            IsLoggedIn = "True";
        else
            IsLoggedIn = "False";

        Details.Username = "";
        Details.Password = "";

    }

}