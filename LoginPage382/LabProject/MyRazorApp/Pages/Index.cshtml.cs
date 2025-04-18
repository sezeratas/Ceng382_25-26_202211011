using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using MyRazorApp.Models; 

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        // Static liste: Tüm sayfalarda veri kaybı olmadan kullanılacak
        private static List<ClassInformationModel> ClassList = new();

        // Verinin sadece bir kez oluşturulmasını sağlayacak flag
        private static bool IsDataInitialized = false;

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
            // Veriyi sadece 1 kez oluştur
            if (!IsDataInitialized)
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

                IsDataInitialized = true;
            }

            // Filtreleme
            var query = ClassList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter))
            {
                query = query.Where(c => c.ClassName.Contains(Filter));
            }

            // Sayfalama
            TotalPages = (int)System.Math.Ceiling(query.Count() / (double)PageSize);

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

        // Yeni sınıf ekleme işlemi (isteğe bağlı)
        public IActionResult OnPostAdd()
        {
            if (NewClass != null)
            {
                NewClass.Id = ClassList.Max(c => c.Id) + 1;
                ClassList.Add(NewClass);
            }

            return RedirectToPage();
        }

        // Sınıf silme işlemi
        public IActionResult OnPostDelete(int id)
        {
            var itemToDelete = ClassList.FirstOrDefault(c => c.Id == id);
            if (itemToDelete != null)
            {
                ClassList.Remove(itemToDelete);
            }

            return RedirectToPage();
        }

        // Sınıf güncelleme işlemi (isteğe bağlı)
        public IActionResult OnPostEdit(ClassInformationModel updatedClass)
        {
            var classItem = ClassList.FirstOrDefault(c => c.Id == updatedClass.Id);
            if (classItem != null)
            {
                classItem.ClassName = updatedClass.ClassName;
                classItem.StudentCount = updatedClass.StudentCount;
                classItem.Description = updatedClass.Description;
            }

            return RedirectToPage();
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