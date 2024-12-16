using e_ticaret1.Data;
using e_ticaret1.Models;
using e_ticaret1.Models.Anasayfa;
using e_ticaret1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace e_ticaret1.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Ürün Listesi
        public async Task<IActionResult> Index()
        {
            // Ürünler ve kullanıcılar
            var products = await _context.Products.ToListAsync();
            var users = await _context.Users.ToListAsync();

            // Ana sayfa verileri
            var sliderImages = await _context.SliderImages.ToListAsync();
            var newProducts = await _context.NewProducts.ToListAsync();
            var campaigns = await _context.Campaigns.ToListAsync();
            var blogFooters = await _context.BlogFooters.ToListAsync();
            var blogPosts = await _context.Blogs.ToListAsync(); // Blog gönderileri

            // ViewModel'e verileri atıyoruz
            var viewModel = new AdminViewModel
            {
                Products = products,
                Users = users,
                SliderImages = sliderImages,
                NewProducts = newProducts,
                Campaigns = campaigns,
                BlogFooters = blogFooters,
                Blogs = blogPosts // Blog gönderilerini ekliyoruz
            };

            // Verileri View'a gönderiyoruz
            return View(viewModel);
        }


        // Ürün Ekleme
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile ImageUrl)
        {

            
                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", ImageUrl.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    }
                    product.ImageUrl = "/images/" + ImageUrl.FileName; // Yüklenen resmin yolu
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            //return View(product);
        }


        // Ürün Güncelleme
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile ImageUrl)
        {

           
                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", ImageUrl.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    }
                    product.ImageUrl = "/images/" + ImageUrl.FileName; // Yüklenen resmin yolu
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            //return View(product);
        }

        // Ürün Silme
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Kullanıcı Listesi
        public IActionResult UserIndex()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        
        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser == null)
                {
                    return NotFound(); // Kullanıcı bulunamazsa hata döndür
                }

                // Güncellenmesi gereken alanlar
                existingUser.UserName = user.UserName; // Kullanıcı adı
                existingUser.Email = user.Email; // Email
                existingUser.Role = user.Role; // Rol
                existingUser.IsActive = user.IsActive; // Aktiflik durumu
                existingUser.CreatedDate = user.CreatedDate; // Oluşturulma tarihi

                // Değişiklikleri kaydet
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(UserIndex));
            }
            return View(user);
        }


        // Kullanıcı Silme
        [HttpGet]
        public IActionResult DeleteUser(string id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(UserIndex));
        }

        public async Task<IActionResult> Settings()
        {
            // Veritabanından 6 farklı model verisini alıyoruz
            var viewModel = new AdminViewModel
            {

                SliderImages = await _context.SliderImages.ToListAsync(),
                NewProducts = await _context.NewProducts.ToListAsync(),
                Campaigns = await _context.Campaigns.ToListAsync(),
                BlogFooters = await _context.BlogFooters.ToListAsync(),
                Blogs = await _context.Blogs.ToListAsync(),
                Products = await _context.Products.ToListAsync() // Product verisini ekliyoruz
            };

            return View(viewModel);
        }


        // Slider Görselleri Güncelleme
        [HttpPost]
        public async Task<IActionResult> UpdateSliderImages(List<SliderImage> SliderImages)
        {
            if (SliderImages != null && SliderImages.Count > 0)
            {
                foreach (var sliderImage in SliderImages)
                {
                    if (sliderImage.Image != null)
                    {
                        // Yeni görseli kaydetmek için bir yol oluşturun
                        var filePath = Path.Combine(_env.WebRootPath, "images", sliderImage.Image.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await sliderImage.Image.CopyToAsync(stream);
                        }

                        // Eski görselin yolunu güncelle
                        sliderImage.ImageUrl = "/images/" + sliderImage.Image.FileName;
                    }

                    // Görseli güncelle
                    _context.SliderImages.Update(sliderImage);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Settings");
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePopularProducts(List<Product> Products, List<IFormFile> Images)
        {
            // Veritabanındaki ürünleri al
            var productsInDb = await _context.Products.ToListAsync();

            // Her ürün için güncelleme işlemi yap
            foreach (var product in Products)
            {
                var productInDb = productsInDb.FirstOrDefault(p => p.Id == product.Id);

                if (productInDb != null)
                {
                    // Ürün bilgilerini güncelle
                    productInDb.Name = product.Name;
                    productInDb.Price = product.Price;

                    // Görsel dosyasını kontrol et ve kaydet
                    var image = Images.FirstOrDefault(i => i.FileName == product.Id.ToString());
                    if (image != null && image.Length > 0)
                    {
                        // Resmi kaydetme işlemi
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", image.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        productInDb.ImageUrl = "/images/" + image.FileName; // Resmin yolu veritabanına kaydedilecek
                    }
                }
            }

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // Admin paneline geri dön
        }




        // Yeni Ürünleri Güncelleme
        [HttpPost]
        public async Task<IActionResult> UpdateNewProducts(List<NewProduct> NewProducts)
        {
            foreach (var newProduct in NewProducts)
            {
                var existingProduct = await _context.Products.FindAsync(newProduct.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = newProduct.Name;
                    existingProduct.Description = newProduct.Description;

                    // Görseli güncelleme
                    if (newProduct.Image != null)
                    {
                        // Dosya yolunu oluştur
                        var filePath = Path.Combine(_env.WebRootPath, "images", newProduct.Image.FileName);

                        // Dosyayı kaydet
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await newProduct.Image.CopyToAsync(stream);
                        }

                        // Veritabanında yalnızca dosya yolunu sakla
                        existingProduct.ImageUrl = "/images/" + newProduct.Image.FileName;
                    }

                    _context.Products.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Settings");
        }



        // Kampanyaları Güncelleme
        [HttpPost]
        public async Task<IActionResult> UpdateCampaigns(List<Campaign> Campaigns)
        {
            foreach (var campaign in Campaigns)
            {
                var existingCampaign = await _context.Campaigns.FindAsync(campaign.Id);
                if (existingCampaign != null)
                {
                    existingCampaign.Title = campaign.Title;
                    existingCampaign.Description = campaign.Description;

                    _context.Campaigns.Update(existingCampaign);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Settings");
        }

        // Blogları Güncelleme
        [HttpPost]
        public async Task<IActionResult> UpdateBlog(List<BlogPost> Blogs)
        {
            foreach (var blog in Blogs)
            {
                var existingBlog = await _context.Blogs.FindAsync(blog.Id);
                if (existingBlog != null)
                {
                    existingBlog.Title = blog.Title;
                    existingBlog.Content = blog.Content;

                    _context.Blogs.Update(existingBlog);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Settings");
        }

        // Blog Footer'ı Güncelleme
        [HttpPost]
        public async Task<IActionResult> UpdateBlogFooter(List<BlogFooter> BlogFooters)
        {
            foreach (var footer in BlogFooters)
            {
                var existingFooter = await _context.BlogFooters.FindAsync(footer.Id);
                if (existingFooter != null)
                {
                    existingFooter.Title = footer.Title;
                    existingFooter.ButtonLink = footer.ButtonLink;

                    _context.BlogFooters.Update(existingFooter);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Settings");
        }

    }
}
