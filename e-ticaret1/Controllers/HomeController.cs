using e_ticaret1.Data;
using e_ticaret1.Models;
using e_ticaret1.Models.Anasayfa;
using e_ticaret1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace e_ticaret1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Giris()
        {
            return View(); // Giris.cshtml görünümünü döndürür
        }

        public async Task<IActionResult> Index()
        {
            

            // Slider resimleri, popüler ürünler, yeni ürünler, kampanyalar, blog gönderileri
            var sliderImages = await _context.SliderImages.ToListAsync();
            var newProducts = await _context.NewProducts.ToListAsync();
            var campaigns = await _context.Campaigns.ToListAsync();
            var blogFooters = await _context.BlogFooters.ToListAsync();
            var blogs = await _context.Blogs.ToListAsync();
            var products = await _context.Products.ToListAsync();

            // Product listesini Popüler Ürünler olarak dönüştür


            // ViewModel'e verileri atıyoruz
            var viewModel = new AdminViewModel
            {
                SliderImages = sliderImages,
                NewProducts = newProducts,
                Campaigns = campaigns,
                BlogFooters = blogFooters,
                Blogs = blogs,
                Products = products
            };

            // Verileri View'a gönderiyoruz
            return View(viewModel);  // View model ile birlikte gönderiyoruz
        }


        public IActionResult Urunler()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
