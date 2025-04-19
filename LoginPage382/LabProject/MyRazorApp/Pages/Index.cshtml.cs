using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using MyRazorApp.Models;
using MyRazorApp.Helpers;
using System.Text.Json;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        // Static list: Will be shared across all instances
        private static List<ClassInformationModel> _classList = new();
        private static bool _isDataInitialized = false;
        private const int PageSize = 10;

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public List<ClassInformationTable> FilteredTable { get; set; } = new();
        public int TotalPages { get; set; }

        [BindProperty]
        public ClassInformationModel NewClass { get; set; }

        public IActionResult OnGet()
        {
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

        public IActionResult OnGetExport(string filter, string selectedColumns, string currentFilter)
        {
            try
            {
                IEnumerable<ClassInformationTable> dataToExport;
                var columns = string.IsNullOrEmpty(selectedColumns) 
                    ? null 
                    : selectedColumns.Split(',').ToList();

                if (filter == "filtered")
                {
                    var query = _classList.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(currentFilter))
                    {
                        query = query.Where(c => c.ClassName.Contains(currentFilter));
                    }

                    dataToExport = query.Select(c => new ClassInformationTable
                    {
                        Id = c.Id,
                        ClassName = c.ClassName,
                        StudentCount = c.StudentCount,
                        Description = c.Description
                    }).ToList();
                }
                else
                {
                    dataToExport = _classList.Select(c => new ClassInformationTable
                    {
                        Id = c.Id,
                        ClassName = c.ClassName,
                        StudentCount = c.StudentCount,
                        Description = c.Description
                    });
                }

                var jsonData = Utils.Instance.ExportToJson(dataToExport, columns);

                return new JsonResult(new 
                { 
                    success = true, 
                    jsonData = jsonData 
                });
            }
            catch
            {
                return new JsonResult(new { success = false });
            }
        }
    }
}