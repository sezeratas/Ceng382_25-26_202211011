// Pages/Login.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using MyRazorApp.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace MyRazorApp.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            // If already logged in, redirect to index
            if (IsUserAuthenticated())
            {
                return RedirectToPage("Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Username and password are required";
                return Page();
            }

            var users = await LoadUsersFromFile();
            var user = users.FirstOrDefault(u => 
                u.Username == Username && 
                u.IsActive && 
                VerifyPassword(Password, u.Password));

            if (user == null)
            {
                ErrorMessage = "Username or password is incorrect";
                return Page();
            }

            // Generate token and store in session/cookies
            var token = GenerateToken();
            var sessionId = HttpContext.Session.Id;

            // Store in session
            HttpContext.Session.SetString("username", user.Username);
            HttpContext.Session.SetString("token", token);
            HttpContext.Session.SetString("session_id", sessionId);

            // Store in cookies
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMinutes(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("username", user.Username, cookieOptions);
            Response.Cookies.Append("token", token, cookieOptions);
            Response.Cookies.Append("session_id", sessionId, cookieOptions);

            return RedirectToPage("Index");
        }

        private async Task<List<User>> LoadUsersFromFile()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
            
            if (!System.IO.File.Exists(path))
            {
                return new List<User>();
            }

            var json = await System.IO.File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private string GenerateToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var tokenBytes = new byte[32];
            rng.GetBytes(tokenBytes);
            return Convert.ToBase64String(tokenBytes);
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // In a real app, you would use proper password hashing like BCrypt
            // This is just for demonstration
            return inputPassword == storedPassword;
        }

        private bool IsUserAuthenticated()
        {
            var usernameFromSession = HttpContext.Session.GetString("username");
            var tokenFromSession = HttpContext.Session.GetString("token");
            var sessionIdFromSession = HttpContext.Session.GetString("session_id");

            var usernameFromCookie = Request.Cookies["username"];
            var tokenFromCookie = Request.Cookies["token"];
            var sessionIdFromCookie = Request.Cookies["session_id"];

            return !string.IsNullOrEmpty(usernameFromSession) &&
                   !string.IsNullOrEmpty(tokenFromSession) &&
                   !string.IsNullOrEmpty(sessionIdFromSession) &&
                   usernameFromSession == usernameFromCookie &&
                   tokenFromSession == tokenFromCookie &&
                   sessionIdFromSession == sessionIdFromCookie;
        }
    }
}