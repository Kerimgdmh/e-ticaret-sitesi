using e_ticaret1.Models;
using e_ticaret1.Models.Anasayfa;

namespace e_ticaret1.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<User> Users { get; set; }

        public List<SliderImage> SliderImages { get; set; }
        public List<NewProduct> NewProducts { get; set; }
        public List<Campaign> Campaigns { get; set; }
        public List<BlogFooter> BlogFooters { get; set; }
        public List<BlogPost> Blogs { get; set; }
    }
}
