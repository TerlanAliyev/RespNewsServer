﻿public class UploadNewsDto
{
    public string NewsTitle { get; set; }
    public string NewsContetText { get; set; }
    public int? NewsCategoryId { get; set; }
    public int? NewsLangId { get; set; }
    public int? NewsOwnerId { get; set; }
    public int? NewsAdminId { get; set; }
    public string NewsTags { get; set; }  // Etiketler string olarak

    public int? NewsRating { get; set; }
    public string? NewsYoutubeLink { get; set; }
    public DateTime? NewsDate { get; set; }

    public List<IFormFile>? NewsPhotos { get; set; }
    public List<IFormFile>? NewsVideos { get; set; }
}
