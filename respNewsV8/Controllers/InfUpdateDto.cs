namespace respNewsV8.Controllers
{
    public class InfUpdateDto
    {
        public int InfId { get; set; }
        public string? InfName { get; set; }
        public IFormFile? InfPhoto { get; set; } // Dosya için IFormFile
        public DateTime? InfPostDate { get; set; }
    }

}
