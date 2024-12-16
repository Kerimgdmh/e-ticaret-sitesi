using System.ComponentModel.DataAnnotations.Schema;

namespace e_ticaret1.Models.Anasayfa
{
    public class NewProduct
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ButtonLink { get; set; }

        // IFormFile, sadece dosya yüklemek için kullanılır
        [NotMapped] // Bu özellik veritabanına kaydedilmez
        public IFormFile Image { get; set; }
    }
}
