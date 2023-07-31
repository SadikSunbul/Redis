using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDistributedCache cache;

        public ValuesController(IDistributedCache cache)
        {
            this.cache = cache;
        }

        [HttpGet("set")]
        public async Task<IActionResult> set(string name, string surneame)
        {
            await cache.SetStringAsync("name", name, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration=TimeSpan.FromSeconds(5)
            });
            await cache.SetAsync("surname", Encoding.UTF8.GetBytes(surneame), options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> get()
        {
            var name = await cache.GetStringAsync("name");
            var surname = await cache.GetAsync("surname");
            var surnamestring = Encoding.UTF8.GetString(surname);
            return Ok(new
            {
                name,
                surnamestring
            });
        }
    }
}
