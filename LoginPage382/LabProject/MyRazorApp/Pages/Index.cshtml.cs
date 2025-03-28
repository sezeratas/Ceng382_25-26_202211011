using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;
using System.Collections.Generic;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<ClassInformationModel> ClassList { get; set; } = new List<ClassInformationModel>();

        [BindProperty]
        public ClassInformationModel NewClass { get; set; }

        public void OnGet()
        {
            // Here we can load existing class data
            ClassList.Add(new ClassInformationModel { Id = 1, ClassName = "Math 101", StudentCount = 30, Description = "Basic Math" });
            ClassList.Add(new ClassInformationModel { Id = 2, ClassName = "History 101", StudentCount = 25, Description = "World History" });
        }

        public IActionResult OnPost()
        {
            // Add new class to the list
            if (ModelState.IsValid)
            {
                NewClass.Id = ClassList.Count + 1; // Simple auto-increment
                ClassList.Add(NewClass);
                return RedirectToPage();
            }
            return Page();
        }
    }
}
