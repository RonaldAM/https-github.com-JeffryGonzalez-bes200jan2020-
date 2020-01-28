using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class CacheController : Controller
    {

        IDistributedCache Cache;

        public CacheController(IDistributedCache cache)
        {
            Cache = cache;
        }


        [ResponseCache(Duration = 130, Location = ResponseCacheLocation.Client)]
        [HttpGet("/time2")]
        public async Task<ActionResult<string>> GetTime2()
        {

            return Ok(new { data = $"The DDDD is {DateTime.Now.ToLongTimeString()}" });
        }

        [HttpGet("/time")]
        public async Task<ActionResult<string>> GetTheTime()
        {
            var time = await Cache.GetAsync("time");
            string newTime = null;
            if(time == null) // it wasn't in the cache. never was, or was removed after it expired
            {
                newTime = DateTime.Now.ToLongTimeString();
                var encodedTime = Encoding.UTF8.GetBytes(newTime);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddSeconds(15));
                await Cache.SetAsync("time", encodedTime, options);
            } else
            {
                newTime = Encoding.UTF8.GetString(time);
            }

            return Ok($"It is now {newTime}");
        }

        [HttpGet("/serverstatus")]
        [ResponseCache(Duration =15, Location = ResponseCacheLocation.Any)]
        public ActionResult<CachedStatus> GetServerStatus()
        {
            return Ok(new CachedStatus { Status = "All good.", CheckedAt = DateTime.Now });
        }
    }

    public class CachedStatus
    {
        public string Status { get; set; }
        public DateTime CheckedAt { get; set; }
    }
}
