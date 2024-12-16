using System.ComponentModel.DataAnnotations;

namespace e_ticaret1.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı gereklidir.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fiyat gereklidir.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat sıfırdan büyük olmalıdır.")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public int Rating { get; set; }
        public string TechnicalDetails { get; set; }
      
    }
}
