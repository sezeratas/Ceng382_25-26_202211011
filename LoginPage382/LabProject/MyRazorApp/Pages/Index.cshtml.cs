using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using MyRazorApp.Models; 
namespace MyRazorApp.Pages;

public class IndexModel : PageModel
{
    public static List<ClassInformationModel> ClassList = new List<ClassInformationModel>();

    private static int _idCounter = 1;

    [BindProperty]
    public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

    [BindProperty]
    public int EditId { get; set; }

    public void OnGet()
    {
        
    }

    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        NewClass.Id = _idCounter++;
        ClassList.Add(NewClass);

        NewClass = new ClassInformationModel();

        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        var item = ClassList.FirstOrDefault(c => c.Id == id);
        if (item != null)
        {
            ClassList.Remove(item);
        }
        return RedirectToPage();
    }

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

//GPT pompt: Razor page kullanarak yapmak istediğim bir proje var. 
// Bana genel bir taslak oluşturur musun? Proje bir sınıf bilgileri uygulaması olacak. 
// Kullanıcı sınıf adı, öğrenci sayısı ve açıklama gibi bilgileri girebilecek. 
// Bu bilgileri listeleyebilecek, düzenleyebilecek ve silebilecek. 
// Ayrıca, kullanıcıdan alınan bilgilerin doğruluğunu kontrol etmek için gerekli validasyonları yapmalısın. 
// Razor Pages kullanarak bu projeyi oluşturmanı istiyorum. 
// Razor Page ile birlikte gerekli model ve sayfa kodlarını da eklemelisin.