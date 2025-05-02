using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using MyRazorApp.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        private static List<ClassInformationModel> _classList = new();
        private static bool _isDataInitialized = false;
        public static int PageSize { get; } = 10;

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public List<string> SelectedColumns { get; set; } = new List<string>();

        public List<ClassInformationTable> FilteredTable { get; set; } = new();
        public int TotalPages { get; set; }

        [BindProperty]
        public ClassInformationModel NewClass { get; set; }

        public IActionResult OnGet()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToPage("Login");
            }

            if (!_isDataInitialized)
            {
                for (int i = 1; i <= 100; i++)
                {
                    _classList.Add(new ClassInformationModel
                    {
                        Id = i,
                        ClassName = $"Class {i}",
                        StudentCount = 20 + (i % 10),
                        Description = $"Sample description {i}"
                    });
                }
                _isDataInitialized = true;
            }

            var query = _classList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter))
            {
                query = query.Where(c => c.ClassName.Contains(Filter));
            }

            TotalPages = (int)System.Math.Ceiling(query.Count() / (double)PageSize);

            FilteredTable = query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(c => new ClassInformationTable
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    StudentCount = c.StudentCount,
                    Description = c.Description
                })
                .ToList();

            return Page();
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

        public IActionResult OnPostAdd()
        {
            if (NewClass != null)
            {
                NewClass.Id = _classList.Max(c => c.Id) + 1;
                _classList.Add(NewClass);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var itemToDelete = _classList.FirstOrDefault(c => c.Id == id);
            if (itemToDelete != null)
            {
                _classList.Remove(itemToDelete);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostEdit(ClassInformationModel updatedClass)
        {
            var classItem = _classList.FirstOrDefault(c => c.Id == updatedClass.Id);
            if (classItem != null)
            {
                classItem.ClassName = updatedClass.ClassName;
                classItem.StudentCount = updatedClass.StudentCount;
                classItem.Description = updatedClass.Description;
            }
            return RedirectToPage();
        }

        public IActionResult OnGetExport(string filter, string selectedColumns, string currentFilter, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var columns = string.IsNullOrEmpty(selectedColumns)
                    ? new List<string>()
                    : selectedColumns.Split(',').ToList();

                var allColumns = new List<string> { "ClassName", "StudentCount", "Description" };
                var finalColumns = columns.Count == 0 ? allColumns : columns;

                var query = _classList.AsQueryable();

                if (!string.IsNullOrWhiteSpace(currentFilter))
                {
                    query = query.Where(c => c.ClassName.Contains(currentFilter));
                }

                if (filter == "currentPage")
                {
                    query = query
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);
                }

                var dataToExport = query
                    .Select(c => new
                    {
                        ClassName = finalColumns.Contains("ClassName") ? c.ClassName : null,
                        StudentCount = finalColumns.Contains("StudentCount") ? c.StudentCount : (int?)null,
                        Description = finalColumns.Contains("Description") ? c.Description : null
                    })
                    .ToList();

                var jsonData = JsonSerializer.Serialize(dataToExport, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                return new JsonResult(new { success = true, jsonData });
            }
            catch
            {
                return new JsonResult(new { success = false });
            }
        }
    }
}