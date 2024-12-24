using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using respNewsV8.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace respNewsV8.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(string username);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly RespNewContext _sql;

        public JwtService(IConfiguration configuration, RespNewContext sql)
        {
            _configuration = configuration;
            _sql = sql;  // DbContext'i buraya ekliyoruz
        }

        public string GenerateJwtToken(string username)
        {
            var user = _sql.Users.FirstOrDefault(u => u.UserNickName == username);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            var claims = new[]
            {
        new Claim("userName", user.UserName), // Kullanıcı adını ekliyoruz
        new Claim("userRole", user.UserRole) ,// userRole claim'ini ekleyin
        new Claim("userId", user.UserId.ToString()) // Kullanıcı ID
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }


}