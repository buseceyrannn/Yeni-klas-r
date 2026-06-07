using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BUSEB.Data;

var builder = WebApplication.CreateBuilder(args);

// 🗄️ Veritabanı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔐 Identity + ROLE SİSTEMİ (KRİTİK DÜZELTME BURADA)
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;

        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>()   // ⭐⭐⭐ BU SATIR HATAYI ÇÖZER
    .AddEntityFrameworkStores<ApplicationDbContext>();

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Session (Sepet için)
builder.Services.AddSession();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// 👑 ADMIN SEED (OTOMATİK ADMIN OLUŞTURMA)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await BUSEB.Data.Seed.AdminSeeder.SeedAsync(services);
}

app.Run();