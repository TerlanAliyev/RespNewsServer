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

builder.Services.AddDistributedMemoryCache(); // Bellek i�i cache


// Veritaban� ba�lant� dizesini yap�land�r�yoruz
builder.Services.AddDbContext<RespNewContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddHttpClient<UnsplashService>();
builder.Services.AddScoped<NewsStatisticsService>();
builder.Services.Configure<UnsplashOptions>(builder.Configuration.GetSection("Unsplash"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// JWT kimlik do�rulama yap�land�rmas�
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
            RoleClaimType = ClaimTypes.Role
        };
    });

// Authorization servisini ekleme
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});




// CORS yap�land�rmas�
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
builder.Services.AddControllersWithViews(); // Bu, TempData'y� ve di�er servisleri kaydeder

// Yetkilendirme
builder.Services.AddAuthorization();

// Controller'lar i�in JSON yap�land�rmas� (Newtonsoft.Json)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase; // Opsiyonel: Camel case kullan�m�
    });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IpHelper>();  // IP yard�mc� s�n�f�n� ekliyoruz

// Uygulama olu�turuluyor
var app = builder.Build();
app.UseCors("AllowAll");

app.UseStaticFiles();
// Hata ay�klama ortam�
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Gerekli middleware'ler
app.UseRouting();

// Kimlik do�rulama ve yetkilendirme
app.UseAuthentication();
app.UseAuthorization();

// API controller'lar�n� haritalama
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=HomePage}/{id?}"); // HomePageController ve HomePage action'�na y�nlendirir

// Uygulama �al��t�r�l�yor
app.Run();
