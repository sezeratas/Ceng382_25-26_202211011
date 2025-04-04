using System.ComponentModel.DataAnnotations;

namespace MyRazorApp.Models
{
    public class ClassInformationModel
    {
        private static int _counter = 1; // ID'yi otomatik arttırmak için

        public ClassInformationModel()
        {
            Id = _counter++;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        public string ClassName { get; set; }

        [Range(1, 1000, ErrorMessage = "Student count must be between 1 and 1000")]
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}