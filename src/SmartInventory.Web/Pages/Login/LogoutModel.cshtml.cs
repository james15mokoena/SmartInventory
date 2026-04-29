using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartInventory.Web.Pages.Login;

public class LogoutModel: PageModel
{
    public IActionResult OnGet()
    {
        // clear user state.        
        HttpContext.Session.Clear();
        TempData["Username"] = null;

        return RedirectToPage("../Index");
    }
}