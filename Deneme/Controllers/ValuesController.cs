using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Redis;
using System.Text.Json;

namespace Deneme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDatabase redisDatabase;

        public ValuesController(IConnectionMultiplexer redisConnection)
        {
            this.redisDatabase = redisConnection.GetDatabase();
        }

        [HttpGet]
        public void Set([FromQuery] Kitap kitap)
        {
            redisDatabase.ListRightPushAsync("kitaplar", JsonSerializer.Serialize(kitap));
          
        }

        public class Kitap
        {
            public string İŞsim { get; set; }
            public int Fiyat { get; set; }
            public string Yazzar { get; set; }
        }

    }
}
