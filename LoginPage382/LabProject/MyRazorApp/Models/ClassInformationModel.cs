using System.ComponentModel.DataAnnotations;

namespace MyRazorApp.Models
{
    public class ClassInformationModel
    {
        private static int _counter = 1; 
        public ClassInformationModel()
        {
            Id = _counter++;
        }
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        public string ClassName { get; set; }

        [Range(1, 1000, ErrorMessage = "Student count must be between 1 and 1000")]
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}

//GPT pompt: Razor page kullanarak yapmak istediğim bir proje var. 
// Bana genel bir taslak oluşturur musun? Proje bir sınıf bilgileri uygulaması olacak. 
// Kullanıcı sınıf adı, öğrenci sayısı ve açıklama gibi bilgileri girebilecek. 
// Bu bilgileri listeleyebilecek, düzenleyebilecek ve silebilecek. 
// Ayrıca, kullanıcıdan alınan bilgilerin doğruluğunu kontrol etmek için gerekli validasyonları yapmalısın. 
// Razor Pages kullanarak bu projeyi oluşturmanı istiyorum. 
// Razor Page ile birlikte gerekli model ve sayfa kodlarını da eklemelisin.
