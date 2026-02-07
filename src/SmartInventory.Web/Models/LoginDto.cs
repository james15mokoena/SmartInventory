using Microsoft.AspNetCore.Mvc;

namespace SmartInventory.Web.Models;

public class LoginDto
{
    /// <summary>
    /// The username.
    /// </summary>
    [BindProperty]
    public string? Username { get; set; } = "";

    /// <summary>
    /// The password.
    /// </summary>
    [BindProperty]
    public string? Password { get; set; } = "";

}