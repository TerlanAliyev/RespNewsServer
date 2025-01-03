using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using respNewsV8.Models;

namespace respNewsV8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoController : ControllerBase
    {
        private readonly RespNewContext _sql;
        public LogoController(RespNewContext sql)
        {
            _sql = sql;
        }

        [HttpGet("logos")]
        public IActionResult Get() { 
            var a=_sql.Logos.Where(x=>x.LogoVisibility==true);
            return Ok(a);
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadLogos([FromForm] Logodto logodto)
        {
            if (logodto.LogoCoverUrl == null || logodto.LogoCoverUrl.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var fileExtension = Path.GetExtension(logodto.LogoCoverUrl.FileName);
            var fileName = Guid.NewGuid().ToString() + fileExtension; // Rastgele isim oluştur
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LogoPhotos", fileName);

            try
            {
                // Dosyayı kaydetme
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await logodto.LogoCoverUrl.CopyToAsync(stream);
                }

                // Veritabanına kaydetme
                var logo = new Logo
                {
                    LogoName = logodto.LogoName,
                    LogoCoverUrl = $"/LogoPhotos/{fileName}", // URL'yi kaydet
                };

                _sql.Logos.Add(logo);  // Infographic nesnesini ekle
                await _sql.SaveChangesAsync();  // Değişiklikleri kaydet

                return Ok(new { message = "File uploaded and saved to database", fileUrl = logo.LogoCoverUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }



        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var logo = _sql.Logos.SingleOrDefault(x => x.LogoId == id);
            _sql.Logos.Remove(logo);
            _sql.SaveChanges();
            return Ok(logo);
        }


        [HttpPut("{id}/visibility")]
        public IActionResult UpdateVisibility(int id, [FromBody] UpdateVisibilityDto visibilityDto)
        {
            var logo = _sql.Logos.SingleOrDefault(x => x.LogoId == id);
            if (logo == null)
            {
                return NotFound();
            }

            // Visibility durumunu güncelle
            logo.LogoVisibility = visibilityDto.IsVisible;

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
