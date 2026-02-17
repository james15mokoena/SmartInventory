using Microsoft.AspNetCore.Mvc;

namespace SmartInventory.Web.Models;

public class LoginDto
{
    /// <summary>
    /// The username.
    /// </summary>
    public string? Username { get; set; } = "";

    /// <summary>
    /// The password.
    /// </summary>
    public string? Password { get; set; } = "";

}