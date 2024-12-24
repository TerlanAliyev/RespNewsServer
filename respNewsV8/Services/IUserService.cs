using Microsoft.Extensions.Logging;
using respNewsV8.Models;

namespace respNewsV8.Services
{
    public interface IUserService
    {
        User? ValidateUser(string userNickName, string password);
        bool IsValidUser(User user);
    }

    public class UserService : IUserService
    {
        private readonly RespNewContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(RespNewContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public User? ValidateUser(string userNickName, string password)
        {
            try
            {
                // Kullanıcıyı veritabanında ara
                var foundUser = _context.Users.FirstOrDefault(u => u.UserNickName == userNickName);

                // Kullanıcı bulunamadıysa hata fırlat
                if (foundUser == null)
                {
                    _logger.LogInformation($"User not found: {userNickName}");
                    throw new UnauthorizedAccessException("User not found");
                }

                // Şifre kontrolü (hashleme yapılmadan)
                if (foundUser.UserPassword != password)
                {
                    _logger.LogInformation($"Invalid password for user: {userNickName}");
                    throw new UnauthorizedAccessException("Invalid password");
                }

                return foundUser; // Kullanıcı geçerli
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while validating user");
                throw;
            }
        }

        public bool IsValidUser(User user)
        {
            try
            {
                var validatedUser = ValidateUser(user.UserNickName, user.UserPassword);
                return validatedUser != null;
            }
            catch
            {
                return false; // Hata durumunda false döndür
            }
        }

    }
}
