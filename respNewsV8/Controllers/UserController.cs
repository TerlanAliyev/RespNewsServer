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





    }
}