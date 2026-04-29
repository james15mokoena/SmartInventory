using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartInventory.Web.Pages.Stock;

public class IndexModel : PageModel
{
    public void OnGet()
    {
        if (HttpContext.Session.GetString("Username") is string username)
            TempData["Username"] = username;
    }
}