using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartInventory.Web.Pages.User;

public class CreateStaffMemberModel : PageModel
{
    public void OnGet()
    {
        if (HttpContext.Session.GetString("Username") is string username)
            TempData["Username"] = username;
    }
}