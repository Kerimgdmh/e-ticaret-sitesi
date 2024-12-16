using e_ticaret1.Models;
using e_ticaret1.Models.Anasayfa; // Bu satırı ekleyin, çünkü Anasayfa sınıflarını kullanacağız
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace e_ticaret1.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Diğer DbSet'ler
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        // Yeni sınıflarınızın DbSet'lerini buraya ekleyin
        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<NewProduct> NewProducts { get; set; }
        public DbSet<BlogPost> Blogs { get; set; }
        public DbSet<BlogFooter> BlogFooters { get; set; }
    }
}
