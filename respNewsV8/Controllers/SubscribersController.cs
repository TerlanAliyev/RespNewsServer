using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using respNewsV8.Models;

namespace respNewsV8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribersController : ControllerBase
    {
        private readonly RespNewContext _sql;

        public SubscribersController(RespNewContext sql)
        {
            _sql = sql ?? throw new ArgumentNullException(nameof(sql));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var subscribers = await _sql.Subscribers
                .ToListAsync();
            return Ok(subscribers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subscribers = await _sql.Subscribers.Where(x=>x.SubId==id)
                .ToListAsync();
            return Ok(subscribers);
        }

        [HttpPost("post")]
        public IActionResult Post([FromForm] Subscriber subscriber)
        {
            try
            {
                subscriber.SubDate = DateTime.Now;
                _sql.Subscribers.Add(subscriber);
                _sql.SaveChanges();

                return Ok(new { message = "Kullanıcı başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu.", error = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var subscriber =  _sql.Subscribers.SingleOrDefault(x => x.SubId == id);
            _sql.Subscribers.Remove(subscriber);
             _sql.SaveChangesAsync();
            return Ok(subscriber);
        }
    }
}
