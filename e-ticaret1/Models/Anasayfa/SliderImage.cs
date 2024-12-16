using System.ComponentModel.DataAnnotations.Schema;

namespace e_ticaret1.Models.Anasayfa
{
    public class SliderImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } // Resim yolu
        [NotMapped]  // Bu özellik veritabanında yer almaz, sadece görsel yüklemek için kullanılır.
        public IFormFile Image { get; set; }
    }
    
}
