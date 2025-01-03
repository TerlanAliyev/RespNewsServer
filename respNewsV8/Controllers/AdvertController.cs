using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using respNewsV8.Models;

namespace respNewsV8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertController : ControllerBase
    {
        private readonly RespNewContext _sql;
        public AdvertController(RespNewContext sql)
        {
            _sql = sql;
        }

        [HttpGet("adverts")]
        public IActionResult Get() { 
            var a=_sql.Adverts.Where(x=>x.AdvertVisibility==true);
            return Ok(a);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadLogos([FromForm] AdvertDto advertDto)
        {
            if (advertDto.AdvertCoverFile == null || advertDto.AdvertCoverFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var fileExtension = Path.GetExtension(advertDto.AdvertCoverFile.FileName);
            var fileName = Guid.NewGuid().ToString() + fileExtension; // Rastgele isim oluştur
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AdvertPhotos", fileName);

            try
            {
                // Kapak dosyasını kaydetme
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await advertDto.AdvertCoverFile.CopyToAsync(stream);
                }

                var advert = new Advert
                {
                    AdvertName = advertDto.AdvertName,
                    AdvertContext = advertDto.AdvertContext,
                    AdvertCoverUrl = $"/AdvertPhotos/{fileName}",
                    AdvertVisibility = advertDto.AdvertVisibility ?? false // Varsayılan değer
                };

                // Video dosyası işleme (eğer varsa)
                if (advertDto.AdvertVideoUrl != null && advertDto.AdvertVideoUrl.Length > 0)
                {
                    var videoExtension = Path.GetExtension(advertDto.AdvertVideoUrl.FileName);
                    var videoFileName = Guid.NewGuid().ToString() + videoExtension;
                    var videoFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AdvertVideos", videoFileName);

                    using (var videoStream = new FileStream(videoFilePath, FileMode.Create))
                    {
                        await advertDto.AdvertVideoUrl.CopyToAsync(videoStream);
                    }

                    advert.AdvertVideoUrl = $"/AdvertVideos/{videoFileName}";
                }

                // Veritabanına kaydetme
                _sql.Adverts.Add(advert); // `Adverts` tablosuna ekleniyor
                await _sql.SaveChangesAsync();

                return Ok(new { message = "File uploaded and saved to database", fileUrl = advert.AdvertCoverUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }




        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var advert = _sql.Adverts.SingleOrDefault(x => x.AdvertId == id);
            _sql.Adverts.Remove(advert);
            _sql.SaveChanges();
            return Ok(advert);
        }


        [HttpPut("{id}/visibility")]
        public IActionResult UpdateVisibility(int id, [FromBody] UpdateVisibilityDto visibilityDto)
        {
            var advert = _sql.Adverts.SingleOrDefault(x => x.AdvertId == id);
            if (advert == null)
            {
                return NotFound();
            }

            // Visibility durumunu güncelle
            advert.AdvertVisibility = visibilityDto.IsVisible;

            // Değişiklikleri kaydet
            var changes = _sql.SaveChanges();

            // Eğer veri güncellenmişse başarılı yanıt döndür
            if (changes > 0)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false, message = "Veri güncellenmedi" });
            }
        }
        // DTO for visibility update
        public class UpdateVisibilityDto
        {
            public bool IsVisible { get; set; }
        }


    }
}
