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
        // Static liste: Tüm sayfalarda veri kaybı olmadan kullanılacak
        private static List<ClassInformationModel> _classList = new();
        private static bool _isDataInitialized = false;
        private const int PageSize = 10;

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
            // Veriyi sadece 1 kez oluştur
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

            // Filtreleme
            var query = _classList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter))
            {
                query = query.Where(c => c.ClassName.Contains(Filter));
            }

            // Sayfalama
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

        public IActionResult OnGetExport(string filter, string selectedColumns, string currentFilter, int currentPage)
        {
            try
            {
                IEnumerable<Dictionary<string, object>> dataToExport;
                var columns = string.IsNullOrEmpty(selectedColumns) 
                    ? new List<string>() 
                    : selectedColumns.Split(',').ToList();

                var query = _classList.AsQueryable();

                // Filtre uygula
                if (!string.IsNullOrWhiteSpace(currentFilter))
                {
                    query = query.Where(c => c.ClassName.Contains(currentFilter));
                }

                // Sayfalama uygula (sadece filtered seçeneği için)
                if (filter == "filtered")
                {
                    query = query
                        .Skip((currentPage - 1) * PageSize)
                        .Take(PageSize);
                }

                dataToExport = query.Select(c => 
                    CreateExportItem(c, columns)
                ).ToList();

                var jsonData = JsonSerializer.Serialize(dataToExport, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

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

        private Dictionary<string, object> CreateExportItem(ClassInformationModel item, List<string> selectedColumns)
        {
            var exportItem = new Dictionary<string, object>();

            // Her zaman ID'yi ekleyelim
            exportItem["id"] = item.Id;

            // Seçili sütunları ekleyelim
            if (selectedColumns.Contains("ClassName"))
            {
                exportItem["className"] = item.ClassName;
            }

            if (selectedColumns.Contains("StudentCount"))
            {
                exportItem["studentCount"] = item.StudentCount;
            }

            if (selectedColumns.Contains("Description"))
            {
                exportItem["description"] = item.Description;
            }

            return exportItem;
        }
    }
}