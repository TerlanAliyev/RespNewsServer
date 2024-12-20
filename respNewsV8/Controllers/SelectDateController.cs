using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using respNewsV8.Models;

namespace respNewsV8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectDateController : ControllerBase
    {
        private readonly RespNewContext _sql;
        public SelectDateController(RespNewContext sql)
        {
            _sql = sql;
        }

        // -https://localhost:44314/api/SelectDate/1/2024-12-19
        [HttpGet("{langId}/{date}")]
        public async Task<IActionResult> GetByDate(int langId,string date)
        {
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return BadRequest("Tarih formatı geçersiz. Lütfen 'yyyy-MM-dd' formatında bir tarih gönderin.");
            }

            var results = await _sql.News
     .Include(x => x.NewsPhotos)
     .Where(x => x.NewsLangId == langId)
     .Where(x => EF.Functions.DateDiffDay(x.NewsDate, parsedDate) == 0)
     .Select(n => new
     {
         n.NewsId,
         n.NewsTitle,
         n.NewsContetText,
         n.NewsLangId,
         n.NewsDate,
         n.NewsTags,
         PhotoUrl = n.NewsPhotos.Select(p => p.PhotoUrl).Take(1).ToList() 
     })
     .ToListAsync();


            return Ok(results);
        }
    }
}
