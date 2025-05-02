// Pages/Logout.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyRazorApp.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Session'ı temizle
            HttpContext.Session.Clear();

            // Login ile ilgili cookie'leri sil
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1), // Geçmiş bir tarih vererek cookie'yi sil
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("username", "", cookieOptions);
            Response.Cookies.Append("token", "", cookieOptions);
            Response.Cookies.Append("session_id", "", cookieOptions);

            return RedirectToPage("Login");
        }
    }
}