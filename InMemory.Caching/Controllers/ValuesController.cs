using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IMemoryCache memoryCache;

        public ValuesController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        //[HttpGet("set/{name}")]
        //public void SetName(string name)
        //{
        //    memoryCache.Set("name", name);//yazma 

        //}
        //[HttpGet]
        //public string GetName()
        //{
        //    //korunaklı ıslem yapıyoruz var ıse name ozellıgı gır ıcerıye
        //    if (memoryCache.TryGetValue<string>("name", out string name))
        //    {
        //        return name.Substring(1, 2);
        //    }
        //    //var name = memoryCache.Get<string>("name");//okuma strıng doner 
        //    return "";
        //}

        [HttpGet("set")]
        public void Setdata()
        {
            
            memoryCache.Set<DateTime>("date", DateTime.Now,
                options: new()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(60),//her 60 sn de verıyı sıler 
                    //DateTime.Now.AddSeconds(60) bir C# programında, şu anki zamanı alır ve üzerine 60 saniye ekler.
                    SlidingExpiration = TimeSpan.FromSeconds(10), //10 sn yı asarsan yine veriyi sil
                    //TimeSpan.FromSeconds(10) C# programında, 10 saniyelik bir zaman aralığını temsil eden TimeSpan nesnesini oluşturur.

                });
        }
        [HttpGet("get")]
        public DateTime GetData()
        {
            return memoryCache.Get<DateTime>("date");
        }
    }
}
