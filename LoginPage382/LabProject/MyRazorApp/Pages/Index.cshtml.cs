using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using MyRazorApp.Models; 

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        private static List<ClassInformationModel> ClassList = new();
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
            // Create synthetic data once
            if (!ClassList.Any())
            {
                for (int i = 1; i <= 100; i++)
                {
                    ClassList.Add(new ClassInformationModel
                    {
                        Id = i,
                        ClassName = $"Class {i}",
                        StudentCount = 20 + (i % 10),
                        Description = $"Sample description {i}"
                    });
                }
            }

            var query = ClassList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter))
            {
                query = query.Where(c => c.ClassName.Contains(Filter));
            }

            TotalPages = (int)Math.Ceiling(query.Count() / (double)PageSize);
            var pagedData = query
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

            FilteredTable = pagedData;
            return Page();
        }
    }
}

//GPT pompt: Razor page kullanarak yapmak istediğim bir proje var. 
// Bana genel bir taslak oluşturur musun? Proje bir sınıf bilgileri uygulaması olacak. 
// Kullanıcı sınıf adı, öğrenci sayısı ve açıklama gibi bilgileri girebilecek. 
// Bu bilgileri listeleyebilecek, düzenleyebilecek ve silebilecek. 
// Ayrıca, kullanıcıdan alınan bilgilerin doğruluğunu kontrol etmek için gerekli validasyonları yapmalısın. 
// Razor Pages kullanarak bu projeyi oluşturmanı istiyorum. 
// Razor Page ile birlikte gerekli model ve sayfa kodlarını da eklemelisin.