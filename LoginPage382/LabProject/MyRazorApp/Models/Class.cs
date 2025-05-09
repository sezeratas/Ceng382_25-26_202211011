using System.ComponentModel.DataAnnotations;

namespace MyRazorApp.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Person count is required")]
        [Range(1, 1000, ErrorMessage = "Person count must be between 1 and 1000")]
        public int PersonCount { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}