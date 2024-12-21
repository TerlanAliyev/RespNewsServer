namespace respNewsV8.Controllers
{
    public class LinkUpdateDto
    {
        public int LinkId { get; set; }

        public string? LinkName { get; set; }

        public string? LinkUrl { get; set; }

        public IFormFile? LinkCoverUrl { get; set; }

        public bool? LinkVisibility { get; set; }
    }

}
