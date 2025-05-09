using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRazorApp.Data;
using MyRazorApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyRazorApp.Pages // Namespace "Pages" olarak güncellendi
{
    public class IndexModel : PageModel
    {
        private readonly SchoolDbContext _context;

        public IndexModel(SchoolDbContext context)
        {
            _context = context;
        }

        // Filtre ve Sayfalama
        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }
        public int PageSize { get; } = 10;

        // Tablo Verileri
       
        public IList<Class> ClassList { get; set; } = new List<Class>(); // Initialize here        public DbSet<Class> Classes { get; set; }

        // Yeni Sınıf Ekleme
        [BindProperty]
        public Class NewClass { get; set; } = new Class(); // Initialize here

            // Sayfa Yükleme
    public async Task OnGetAsync()
    {
        try
        {
            var query = _context.Classes.Where(c => c.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(Filter))
                query = query.Where(c => c.Name.Contains(Filter));

            TotalPages = (int)System.Math.Ceiling(await query.CountAsync() / (double)PageSize);

            ClassList = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ClassList = new List<Class>();
        }
    }

        // CRUD Operasyonları
        public async Task<IActionResult> OnPostAddAsync()
        {
            Console.WriteLine("OnPostAddAsync started"); // Debug 1
            
            /*if (!ModelState.IsValid)
            {
                Console.WriteLine($"ModelState invalid: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}"); // Debug 2
                await OnGetAsync();
                return Page();
            }*/

            try
            {
                Console.WriteLine($"Attempting to add: {NewClass.Name}, {NewClass.PersonCount}, {NewClass.Description}"); // Debug 3
                
                // Manuel ID atamasını kaldırın (DB'de auto-increment olmalı)
                NewClass.IsActive = true;
                
                _context.Classes.Add(NewClass);
                int result = await _context.SaveChangesAsync();
                
                Console.WriteLine($"SaveChanges result: {result}"); // Debug 4
                
                // Formu resetle
                NewClass = new Class(); 
                
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.ToString()}"); // Debug 5
                ModelState.AddModelError(string.Empty, "Error adding class. See logs for details.");
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var classToDelete = await _context.Classes.FindAsync(id);
            if (classToDelete != null)
            {
                classToDelete.IsActive = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}