using e_ticaret1.Data;
using e_ticaret1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace e_ticaret1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context ?? throw new ArgumentNullException(nameof(context)); // context'i burada atayın
        }
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Kullanıcı kaydedildikten sonra sepet kaydı oluştur
                    var cartItem = new CartItem
                    {
                        ProductId = 1,
                        Name = string.Empty, // Başlangıçta boş
                        Price = 0, // Başlangıçta sıfır
                        ImageUrl = string.Empty, // Başlangıçta boş
                        Quantity = 0, // Başlangıçta sıfır
                        UserId = user.Id, // IdentityUser'dan alınan UserId
                    };

                    // Sepet kaydını veritabanına ekle
                    _context.CartItems.Add(cartItem);
                    await _context.SaveChangesAsync(); // Değişiklikleri kaydet

                    return RedirectToAction("Index", "Home"); // Ürünler sayfasına yönlendir
                }

                // Hatalar varsa ModelState'e ekle
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Kayıt işlemi başarısız olduğunda tekrar formu göster
            return View("giris");
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); // Ürünler sayfasına yönlendir
                }
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
            }
            return View("giris"); // Giriş işlemi başarısız olduğunda tekrar formu göster
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}