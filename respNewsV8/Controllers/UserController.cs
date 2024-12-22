using System.Runtime.InteropServices.Marshalling;
using Microsoft.AspNetCore.Mvc;
using respNewsV8.Models;
using respNewsV8.Services;

namespace respNewsV8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly RespNewContext _sql;


        public UserController(IUserService userService, IJwtService jwtService, RespNewContext sql)
        {
            _userService = userService;
            _jwtService = jwtService;
            _sql = sql;
        }


        [HttpGet("users")]
        public IActionResult Get()
        {
            var users = _sql.Users.ToList();
            return Ok(users);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                // ID'ye göre kullanıcıyı bul
                var user = _sql.Users.SingleOrDefault(x => x.UserId == id);

                // Kullanıcı bulunamadıysa
                if (user == null)
                {
                    return NotFound(new { message = "Kullanıcı bulunamadı." });
                }

                // Kullanıcıyı sil
                _sql.Users.Remove(user);
                _sql.SaveChanges();

                return Ok(new { message = "Kullanıcı başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu.", error = ex.Message });
            }
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            try
            {
                if (login == null || string.IsNullOrEmpty(login.UserNickName) || string.IsNullOrEmpty(login.UserPassword))
                {
                    return BadRequest(new { Message = "Username and password are required" });
                }

                // Kullanıcı doğrulama
                var user = _sql.Users.FirstOrDefault(u => u.UserNickName == login.UserNickName);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                if (user == null || !_userService.IsValidUser(new User { UserNickName = login.UserNickName, UserPassword = login.UserPassword }))
                {
                    return Unauthorized(new { Message = "Invalid username or password" });
                }

                // JWT Token üretimi
                var tokenString = _jwtService.GenerateJwtToken(user.UserNickName);

                // Cookie ayarları
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // JavaScript erişimini engelle
                    Secure = true, // HTTPS üzerinden gönder
                    SameSite = SameSiteMode.Strict, // CSRF saldırılarını önle
                    Expires = DateTime.Now.AddMinutes(30) // Geçerlilik süresi
                };

                // Cookie'ye ek bilgiler ekleyin
                Response.Cookies.Append("jwtToken", tokenString, cookieOptions);
                Response.Cookies.Append("userRole", user.UserRole, cookieOptions);
                Response.Cookies.Append("userName", user.UserName, cookieOptions);

                return Ok(new { Message = "Login successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpGet("getUserId")]
        public IActionResult GetUserId()
        {
            var userId = HttpContext.Session.GetString("UserId");

            return Ok(new { userId});
        }



        [HttpPost]
        public IActionResult Post([FromForm] User user)
        {
            user.UserRole = "FullAdmin";
            try
            {
                _sql.Users.Add(user);
                _sql.SaveChanges();
                return Ok(new { message = "Kullanıcı başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcı eklenirken bir hata oluştu.", error = ex.Message });
            }
        }


    }
}