using Microsoft.AspNetCore.Mvc;
using respNewsV8.Models;

namespace respNewsV8.Services
{
    // Kullanıcı hizmet arayüzü
    public interface IUserService
    {
        User? ValidateUser(string userNickName, string password);
        bool IsValidUser(User user);
    }

    // Kullanıcı hizmet sınıfı
    public class UserService : IUserService
    {
        private readonly RespNewContext _context;

        // Bağımlılık enjeksiyonu ile veritabanı bağlamını al
        public UserService(RespNewContext context)
        {
            _context = context;
        }

        // Kullanıcı doğrulama metodu
        public User? ValidateUser(string userNickName, string password)
        {
            try
            {
                // Kullanıcıyı veritabanında ara
                var foundUser = _context.Users
                    .FirstOrDefault(u => u.UserNickName == userNickName);

                // Kullanıcı bulunamadıysa veya şifre yanlışsa
                if (foundUser == null || foundUser.UserPassword != password)
                {
                    return null;
                }

                // Kullanıcı rolünü kontrol et
                if (foundUser.UserRole != "FullAdmin" && foundUser.UserRole!="Admin") // İstenilen rolü kontrol edebilirsiniz
                {
                    return null;
                }

                // Kullanıcı bilgilerini döndür
                return foundUser;
            }
            catch (Exception ex)
            {
                // Hataları konsola yaz
                Console.WriteLine($"Hata: {ex.Message}");
                return null;
            }
        }



        // Kullanıcı geçerliliğini kontrol eden metod
        public bool IsValidUser(User user)
        {
            try
            {
                // Kullanıcıyı veritabanında ara
                var foundUser = _context.Users
                    .FirstOrDefault(u => u.UserNickName == user.UserNickName);

                // Kullanıcı bulunamadıysa veya şifre yanlışsa
                if (foundUser == null || foundUser.UserPassword != user.UserPassword)
                {
                    return false;
                }

                // Kullanıcı rolünü kontrol et
                if (foundUser.UserRole != "FullAdmin")
                {
                    return false;
                }

                return true; // Kullanıcı geçerli
            }
            catch (Exception ex)
            {
                // Hataları konsola yaz
                Console.WriteLine($"Hata: {ex.Message}");
                return false;
            }
        }
    }
}
