using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using respNewsV8.Models;
using respNewsV8.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using static respNewsV8.Controllers.NewspaperController;

namespace respNewsV8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinksController : ControllerBase
    {
        private readonly RespNewContext _sql;

        public LinksController(RespNewContext sql)
        {
            _sql = sql;
        }


        [HttpGet]
        public async Task<IActionResult>Get()
        { 
            var links=_sql.AdditionalLinks.Where(x=>x.LinkVisibility==true);
            return Ok(links);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var links = _sql.AdditionalLinks.SingleOrDefault(x=>x.LinkId==id);
            return Ok(links);
        }

        [HttpDelete("id/{id}")]
        public IActionResult DeleteLink(int id)
        {
            var a = _sql.AdditionalLinks.SingleOrDefault(x => x.LinkId == id);
            _sql.AdditionalLinks.Remove(a);
            _sql.SaveChanges();
            return Ok();
        }

        [HttpPut("edit/{id}")]
        public IActionResult UpdateNews(int id, [FromForm] LinkUpdateDto linkUpdateDto)
        {
            try
            {
                var existingLink = _sql.AdditionalLinks.SingleOrDefault(x => x.LinkId == id);

                if (existingLink == null)
                {
                    return NotFound(new { Message = "Güncellenecek link bulunamadı." });
                }

                // Metin alanlarını güncelle
                existingLink.LinkName = linkUpdateDto.LinkName;
                existingLink.LinkUrl = linkUpdateDto.LinkUrl;

                // Eğer yeni bir fotoğraf yüklenmişse
                if (linkUpdateDto.LinkCoverUrl != null && linkUpdateDto.LinkCoverUrl.Length > 0)
                {
                    Console.WriteLine($"Dosya adı: {linkUpdateDto.LinkCoverUrl.FileName}");
                    Console.WriteLine($"Dosya boyutu: {linkUpdateDto.LinkCoverUrl.Length}");

                    // Fotoğrafı kaydetmek için dizin oluştur
                    var uploadsFolder = Path.Combine("wwwroot" ,"LinkPhotos");
                    Directory.CreateDirectory(uploadsFolder);

                    // Dosya adı oluştur
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + linkUpdateDto.LinkCoverUrl.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Dosyayı kaydet
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        linkUpdateDto.LinkCoverUrl.CopyTo(stream);
                    }

                    // Veritabanındaki fotoğraf yolunu güncelle
                    existingLink.LinkCoverUrl = $"/Linkphotos/{uniqueFileName}";
                }

                // Veritabanını güncelle
                _sql.SaveChanges();

                return Ok(new { Message = "Link başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return StatusCode(500, new { Message = "Bir hata oluştu. Lütfen tekrar deneyin." });
            }
        }




        [HttpPut("{id}/visibility")]
        public IActionResult UpdateVisibility(int id, [FromBody] UpdateVisibilityDto model)
        {
            // İlgili kategoriyi ID'ye göre sorgula
            var paper = _sql.AdditionalLinks.SingleOrDefault(x => x.LinkId == id);

            if (paper == null)
            {
                return NotFound(new { success = false, message = "Pdf bulunamadı" });
            }

            // Gelen DTO'daki görünürlük durumunu kategoriye ata
            paper.LinkVisibility = model.IsVisible;

            // Veritabanı değişikliklerini kaydet
            var changes = _sql.SaveChanges();

            // Güncelleme başarılıysa olumlu bir yanıt döndür
            if (changes > 0)
            {
                return Ok(new { success = true, message = "Pdf durumu güncellendi" });
            }

            // Güncelleme başarısızsa hata mesajı döndür
            return BadRequest(new { success = false, message = "Pdf durumu güncellenemedi" });
        }


        [HttpPost("upload")]
        public async Task<IActionResult> Post([FromForm] AdditionalLinkModel model)
        {
            if (model == null || model.LinkCoverFile == null)
            {
                return BadRequest("Invalid data.");
            }

            // Save the file
            var filePath = await SaveFileAsync(model.LinkCoverFile, "LinkPhotos");

            // Create the AdditionalLink instance
            var additionalLink = new AdditionalLink
            {
                LinkName = model.LinkName,
                LinkUrl = model.LinkUrl,
                LinkCoverUrl = filePath,
                LinkVisibility = true,
            };

            _sql.AdditionalLinks.Add(additionalLink);
            _sql.SaveChanges();

            return Ok(additionalLink);
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folderName}/{uniqueFileName}";
        }
    }


    public class AdditionalLinkModel
    {
        public string? LinkName { get; set; }
        public string? LinkUrl { get; set; }
        public IFormFile? LinkCoverFile { get; set; }
        public bool? LinkVisibility { get; set; }
    }
    public class UpdateVisibilityDto
    {
        public bool IsVisible { get; set; }
    }
}
