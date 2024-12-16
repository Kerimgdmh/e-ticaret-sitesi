using Microsoft.AspNetCore.Mvc;
using e_ticaret1.Data;
using e_ticaret1.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace e_ticaret1.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly int pageSize = 25; // Sayfa başına gösterilecek ürün sayısı

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Search(string query, int page = 1)
        {
            // Null veya boş sorgu kontrolü
            if (string.IsNullOrEmpty(query))
            {
                // Arama sorgusu yoksa, tüm ürünleri döndür
                return RedirectToAction("urunler");
            }

            query = query.ToLower(); // Küçük harfe çevir

            // Ürünleri arama sorgusuna göre filtrele
            var products = _context.Products
                                   .Where(p => p.Name.ToLower().Contains(query) || p.Description.ToLower().Contains(query))
                                   .OrderBy(p => p.Id)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();

            // Arama sorgusuna göre toplam ürün sayısını al
            int totalItems = _context.Products
                                     .Count(p => p.Name.ToLower().Contains(query) || p.Description.ToLower().Contains(query));

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Arama sonuçlarını ViewBag'e atıyoruz
            ViewBag.SearchResults = products;
            ViewBag.Query = query;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            // Arama sonuçlarını göstermek için urunler.cshtml sayfasına yönlendiriyoruz
            return View("urunler", products);
        }





        public IActionResult urunler(int page = 1)
        {
            // AppDbContext ile ürünleri sorgula
            var products = _context.Products
                                   .OrderBy(p => p.Id)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();

            int totalItems = _context.Products.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(products);
        }

        public IActionResult ClearProducts()
        {
            _context.Products.RemoveRange(_context.Products); // Ürünleri sil
            _context.SaveChanges();
            return RedirectToAction("Index"); // Silme işlemi sonrası listele
        }

        public async Task<IActionResult> AddProducts()
        {
            if (!_context.Products.Any()) // Eğer ürün yoksa
            {
                var products = new List<Product>
                {
                    new Product { Name = "Ürün 1", Price = 100, Description = "IDS 5mp Sony Lensli 1080p 18 Nano Led Gece Görüşlü Iç Mekan Fullhd Dome Güvenlik Kamerası", ImageUrl = "https://www.siteyapicieticaret.net/wp-content/uploads/2017/08/asonic.jpg" },
                    new Product { Name = "Ürün 2", Price = 200, Description = "IDS 5mp Sony Lensli 1080p 18 Nano Led Gece Görüşlü Iç Mekan Fullhd Dome Güvenlik Kamerası", ImageUrl = "https://www.siteyapicieticaret.net/wp-content/uploads/2017/08/asonic.jpg" },
                    new Product { Name = "Ürün 3", Price = 300, Description = "IDS 5mp Sony Lensli 1080p 18 Nano Led Gece Görüşlü Iç Mekan Fullhd Dome Güvenlik Kamerası", ImageUrl = "https://www.siteyapicieticaret.net/wp-content/uploads/2017/08/asonic.jpg" },
                    new Product { Name = "Ürün 4", Price = 400, Description = "IDS 5mp Sony Lensli 1080p 18 Nano Led Gece Görüşlü Iç Mekan Fullhd Dome Güvenlik Kamerası", ImageUrl = "https://www.siteyapicieticaret.net/wp-content/uploads/2017/08/asonic.jpg" }
                    // Daha fazla ürün ekleyebilirsiniz
                };

                await _context.Products.AddRangeAsync(products); // Ürünleri ekle
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            return RedirectToAction("urunler"); // İşlem sonrası listele
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
    }
}
