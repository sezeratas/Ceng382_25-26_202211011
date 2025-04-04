using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using MyRazorApp.Models; // Model sınıfına erişim

namespace MyRazorApp.Pages;

public class IndexModel : PageModel
{
    // Static liste, veriler burada tutulur
    public static List<ClassInformationModel> ClassList = new List<ClassInformationModel>();

    // Static sayaç
    private static int _idCounter = 1;

    // Formdan gelen veriyi yakalayacak property
    [BindProperty]
    public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

    // Düzenleme için formu dolduracak geçici property
    [BindProperty]
    public int EditId { get; set; }

    public void OnGet()
    {
        // Sayfa ilk açıldığında çalışır
    }

    // Ekleme işlemi
    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Yeni sınıfa benzersiz bir Id atayın
        NewClass.Id = _idCounter++;
        ClassList.Add(NewClass);

        // NewClass nesnesini sıfırla
        NewClass = new ClassInformationModel();

        return RedirectToPage();
    }

    // Silme işlemi
    public IActionResult OnPostDelete(int id)
    {
        var item = ClassList.FirstOrDefault(c => c.Id == id);
        if (item != null)
        {
            ClassList.Remove(item);
        }
        return RedirectToPage();
    }

    // Edit için formu doldurma (isteğe bağlı eklenecek)
    public IActionResult OnPostEdit(int id)
    {
        var item = ClassList.FirstOrDefault(c => c.Id == id);
        if (item != null)
        {
            NewClass = new ClassInformationModel
            {
                Id = item.Id,
                ClassName = item.ClassName,
                StudentCount = item.StudentCount,
                Description = item.Description
            };
        }
        return Page();
    }

    // Güncelleme işlemi
    public IActionResult OnPostUpdate()
    {
        var item = ClassList.FirstOrDefault(c => c.Id == NewClass.Id);
        if (item != null)
        {
            item.ClassName = NewClass.ClassName;
            item.StudentCount = NewClass.StudentCount;
            item.Description = NewClass.Description;
        }
        return RedirectToPage();
    }
}