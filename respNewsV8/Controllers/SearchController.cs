using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using respNewsV8.Models;
using respNewsV8.Services;

namespace respNewsV8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly RespNewContext _sql;
        public SearchController(RespNewContext sql)
        {
            _sql = sql;

        }
        private int? GetLanguageIdByCode(int langCode)
        {
            var language = _sql.Languages.FirstOrDefault(l => l.LanguageId == langCode);
            if (language == null)
            {
                Console.WriteLine($"Dil kodu bulunamadı: {langCode}");
                return null;
            }
            return language.LanguageId;
        }
        //For news
        /* -https://localhost:44314/api/search/1/yusif/0*/
        [HttpGet("{langCode}/{query}/{pageNumber?}")]
        public async Task<IActionResult> GetSearch(int langCode, string query, int pageNumber = 0)
        {
            // Veritabanı sorgusu
            var queryable = _sql.News
                .Include(x => x.NewsAdmin)
                .Include(x => x.NewsOwner)
                .Include(x => x.NewsCategory)
                .Include(x => x.NewsLang)
                .Where(x => x.NewsStatus == true);

            // LangId için filtreleme yap (langCode null veya 0 değilse)
            if (langCode > 0)
            {
                queryable = queryable.Where(x => x.NewsLangId == langCode);
            }

            // Diğer filtreleme işlemleri
            queryable = queryable.Where(x =>
                x.NewsTitle.Contains(query) ||
                x.NewsContetText.Contains(query) ||
                x.NewsCategory.CategoryName.Contains(query) ||
                x.NewsOwner.OwnerName.Contains(query) ||
                x.NewsAdmin.UserName.Contains(query) ||
                x.NewsTags.Contains(query)
            );

            var results = await queryable
                .Select(n => new
                {
                    n.NewsId,
                    n.NewsTitle,
                    n.NewsDate,
                    n.NewsCategory.CategoryName,
                    n.NewsLang.LanguageName,
                    n.NewsTags,
                    n.NewsOwner.OwnerName,
                    n.NewsAdmin.UserName,
                    n.NewsPhotos,
                })
                .Skip(pageNumber * 10)
                .Take(10)
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound("No results found.");
            }

            return Ok(results);
        }

        //For NewsPaper
        /* -https://localhost:44314/api/search/newspaper/yusif/0  */
        [HttpGet("newspaper/{query}/{pageNumber?}")]
        public async Task<IActionResult> GetSearchNewsPaper(string query, int pageNumber = 0)
        {
            // Veritabanı sorgusu
            var queryable = _sql.Newspapers
                .Where(x => x.NewspaperStatus == true);


            // Diğer filtreleme işlemleri
            queryable = queryable.Where(x =>
                x.NewspaperTitle.Contains(query)
            );

            var results = await queryable
                .Select(n => new
                {
                    n.NewspaperId,
                    n.NewspaperTitle,
                    n.NewspaperCoverUrl,
                    n.NewspaperDate,
                    n.NewspaperStatus,
                })
                .Skip(pageNumber * 10)
                .Take(10)
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound("No results found.");
            }

            return Ok(results);
        }




        //For NewsInfog
        /* -https://localhost:44314/api/search/infographics/c/0  */
        [HttpGet("infographics/{query}/{pageNumber?}")]
        public async Task<IActionResult> GetSearchInfographics(string query, int pageNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            var queryable = _sql.İnfographics.AsQueryable();

            queryable = queryable.Where(x => x.InfName.Contains(query)); // Doğru alan adını kontrol edin.

            var results = await queryable
                .Select(n => new
                {
                    n.InfId,
                    n.InfName,
                    n.InfPostDate,
                    n.InfPhoto,
                })
                .Skip(pageNumber * 10)
                .Take(10)
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound("No results found.");
            }

            return Ok(results);
        }





        //For Newsvideos
        /* -https://localhost:44314/api/search/videos/c/0  */
        [HttpGet("videos/{query}/{pageNumber?}")]
        public async Task<IActionResult> GetSearchVideos(string query, int pageNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            var queryable = _sql.Ytvideos.AsQueryable();

            queryable = queryable.Where(x => x.VideoTitle.Contains(query));

            var results = await queryable
                .Select(n => new
                {
                    n.VideoId,
                    n.VideoTitle,
                    n.VideoUrl,
                    n.VideoDate,
                    n.VideoStatus,
                })
                .Skip(pageNumber * 10)
                .Take(10)
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound("No results found.");
            }

            return Ok(results);
        }


        //For links
        /* -https://localhost:44314/api/search/videos/c/0  */
        [HttpGet("link/{query}/{pageNumber?}")]
        public async Task<IActionResult> GetSearchLinks(string query, int pageNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            var queryable = _sql.AdditionalLinks.AsQueryable();

            queryable = queryable.Where(x => x.LinkName.Contains(query));

            var results = await queryable
                .Select(n => new
                {
                    n.LinkId,
                    n.LinkName,
                    n.LinkUrl,
                    n.LinkCoverUrl,
                    n.LinkVisibility,
                })
                .Skip(pageNumber * 10)
                .Take(10)
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound("No results found.");
            }

            return Ok(results);
        }



        //For Users
        /* -https://localhost:44314/api/search/user/c/0  */
        [HttpGet("user/{query}/{pageNumber?}")]
        public async Task<IActionResult> GetSearchUsers(string query, int pageNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            var queryable = _sql.Users.AsQueryable();

            queryable = queryable.Where(x => x.UserName.Contains(query) || x.UserNickName.Contains(query));

            var results = await queryable
                .Select(n => new
                {
                    n.UserId,
                    n.UserName,
                    n.UserRole,
                    n.UserEmail,
                    n.UserNickName,
                })
                .Skip(pageNumber * 10)
                .Take(10)
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound("No results found.");
            }

            return Ok(results);
        }




        //For Sub
        /* -https://localhost:44314/api/search/user/c/0  */
        [HttpGet("sub /{query}/{pageNumber?}")]
        public async Task<IActionResult> GetSearchSub(string query, int pageNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            var queryable = _sql.Subscribers.AsQueryable();

            queryable = queryable.Where(x => x.SubEmail.Contains(query));

            var results = await queryable
                .Select(n => new
                {
                    n.SubId,
                    n.SubEmail,
                    n.SubDate,
                })
                .Skip(pageNumber * 10)
                .Take(10)
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound("No results found.");
            }

            return Ok(results);
        }


    }
}
