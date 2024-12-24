namespace respNewsV8.Controllers
{
    public class TokenDto
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public DateTime Expiration { get; set; }
    }
}
