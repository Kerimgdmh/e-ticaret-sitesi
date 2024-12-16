using e_ticaret1.Data;
using e_ticaret1.Models;
using e_ticaret1.Models.Anasayfa;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Entity Framework Core için DbContext ayarý
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity ayarlarý
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// **Session'ı ekleyin** (Eğer Session kullanıyorsanız)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication(); // Kullanýcý doðrulamasý
app.UseAuthorization();

// Admin rolü ve admin kullanýcýsýný oluþtur
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        // Admin rolünü oluþtur
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Admin kullanýcýsýný oluþtur
        var adminEmail = "admin@example.com"; // Admin email adresi
        var adminPassword = "Admin123!"; // Admin için güçlü bir þifre

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var newUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                Role = 1, // Eðer böyle bir rolünüz varsa ayarlayýn
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            var result = await userManager.CreateAsync(newUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, "Admin");
            }
        }
    }
    catch (Exception ex)
    {
        // Hata durumunda loglama yapabilirsiniz
        Console.WriteLine($"Admin oluþturma sýrasýnda hata: {ex.Message}");
    }
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
