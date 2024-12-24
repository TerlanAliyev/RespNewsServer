using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using respNewsV8.Helper;
using respNewsV8.Models;
using respNewsV8.Services;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using DeviceDetectorNET.Parser.Device;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache(); // Bellek içi cache


// Veritabaný baðlantý dizesini yapýlandýrýyoruz
builder.Services.AddDbContext<RespNewContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddHttpClient<UnsplashService>();
builder.Services.AddScoped<NewsStatisticsService>();
builder.Services.Configure<UnsplashOptions>(builder.Configuration.GetSection("Unsplash"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// JWT kimlik doðrulama yapýlandýrmasý
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            RoleClaimType = "userRole" // Role claim'i olarak userRole kullanýlýyor
        };
    });


// Authorization servisini ekleme
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});




// CORS yapýlandýrmasý
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://localhost:44395") // Frontend URL'sini belirtin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Credentials'ý izinli hale getir
    });
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session zaman aþýmý
    options.Cookie.HttpOnly = true; // JavaScript eriþimini engelle
    options.Cookie.IsEssential = true; // GDPR uyumluluðu için gerekli
});
builder.Services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
builder.Services.AddControllersWithViews(); // Bu, TempData'yý ve diðer servisleri kaydeder
builder.Services.AddDistributedMemoryCache(); // Oturum verilerini saklamak için
builder.Services.AddSession(); // Session'ý etkinleþtir
builder.Services.AddControllers();


// Yetkilendirme
builder.Services.AddAuthorization();

// Controller'lar için JSON yapýlandýrmasý (Newtonsoft.Json)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase; // Opsiyonel: Camel case kullanýmý
    });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IpHelper>();  // IP yardýmcý sýnýfýný ekliyoruz

// Uygulama oluþturuluyor
var app = builder.Build();
app.UseCors("AllowSpecificOrigin");




app.UseCors("AllowSpecificOrigin");
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication(); // Kimlik doðrulama middleware'i
app.UseAuthorization();  // Yetkilendirme middleware'i
app.MapControllers();


app.UseStaticFiles();
// Hata ayýklama ortamý
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=HomePage}/{id?}"); // HomePageController ve HomePage action'ýna yönlendirir

// Uygulama çalýþtýrýlýyor
app.Run();
