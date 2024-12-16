using Microsoft.AspNetCore.Mvc;
using e_ticaret1.Models;
using e_ticaret1.Models.Extensions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using e_ticaret1.Data;

namespace e_ticaret1.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private const string CartSessionKey = "CartSessionKey";

        public CartController(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // context'i burada atayın
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Giriş yapan kullanıcının ID'sini al
            if (userId == null)
            {
                return RedirectToAction("giris", "Account");
            }
            var cartItems = _context.CartItems.Where(c => c.UserId == userId).ToList(); // Kullanıcıya ait sepet öğelerini getir
            return View(cartItems); // Görünüme kullanıcıya ait sepet verilerini gönder
        }


        [HttpPost]
        public IActionResult AddToCart(int id, string name, decimal price, string imageUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kullanıcı giriş yapmamışsa giriş sayfasına yönlendir
            if (userId == null)
            {
                return RedirectToAction("Giris", "Account");
            }

            // Kullanıcıya ait mevcut sepet öğelerini veritabanından al
            var existingItem = _context.CartItems
                .FirstOrDefault(item => item.ProductId == id && item.UserId == userId);

            if (existingItem != null)
            {
                // Eğer ürün zaten sepette varsa, miktarını artır
                existingItem.Quantity += 1;
                _context.CartItems.Update(existingItem); // Güncellemeyi veritabanına kaydet
            }
            else
            {
                // Ürün yoksa, yeni bir sepet öğesi oluştur
                var newItem = new CartItem
                {
                    ProductId = id,
                    Name = name,
                    Price = price,
                    ImageUrl = imageUrl,
                    Quantity = 1,
                    UserId = userId
                };
                _context.CartItems.Add(newItem); // Yeni öğeyi veritabanına ekle
            }

            // Değişiklikleri kaydet
            _context.SaveChanges();

            return RedirectToAction("Index"); // Sepet sayfasına yönlendir
        }




        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("giris", "Account");
            }

            var itemToRemove = _context.CartItems
                .FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

            if (itemToRemove != null)
            {
                _context.CartItems.Remove(itemToRemove);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int change)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("giris", "Account");
            }

            var item = _context.CartItems
                .FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

            if (item != null)
            {
                item.Quantity += change;

                if (item.Quantity < 1)
                {
                    item.Quantity = 1;
                }

                _context.CartItems.Update(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("giris", "Account");
            }

            var itemsToRemove = _context.CartItems.Where(c => c.UserId == userId).ToList();
            _context.CartItems.RemoveRange(itemsToRemove);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
