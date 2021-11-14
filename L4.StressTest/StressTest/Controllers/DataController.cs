using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StressTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly int beta = 1;
        private readonly bool cacheFlushing = true;
        private readonly string connectionString;


        public DataController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            connectionString = "Server = DESKTOP-G3EQEMK\\SQLEXPRESS; Trusted_Connection = True; MultipleActiveResultSets = true; Integrated Security=SSPI; DataBase = HSA";
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int id)
        {
            string result;

            _memoryCache.TryGetValue(id, out CacheObject cacheObject);
            if (cacheObject != null)
            {
                var add = cacheObject.Delta * beta * Math.Log(new Random().Next());
                if (cacheFlushing && DateTime.UtcNow.AddSeconds(-add) >= cacheObject.ExpirationDate)
                {
                    _memoryCache.Remove(cacheObject);
                }
                else
                {
                    return Ok(cacheObject.Value);
                }
            }

            var startTime = DateTime.UtcNow;

            var sql = $"select value+value+value+value from StressTest where id = {id};";
            using var connection = new SqlConnection(connectionString);
            try
            {
                result = connection.QuerySingle<string>(sql);
            }
            catch
            {
                return StatusCode(500);
            }

            var delta = (DateTime.UtcNow - startTime).TotalSeconds;

            var expSeconds = 10;
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(expSeconds));
            _memoryCache.Set(id, new CacheObject(DateTime.UtcNow.AddSeconds(expSeconds), result, delta), cacheEntryOptions);
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] SaveValue value)
        {
            var insertQuery = $"INSERT INTO StressTest (value) VALUES ('{value.Value}');";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(insertQuery);
            }

            return true;
        }

        private void CpuLoad(int seconds)
        {
            var end = DateTime.Now + TimeSpan.FromSeconds(seconds);
            while (DateTime.Now < end)
                /* nothing here */
                ;
        }
    }

    public class SaveValue
    {
        public string Value { get; set; }
    }

    public class CacheObject
    {
        public double Delta { get; }

        public DateTime ExpirationDate { get; }

        public string Value { get; }

        public double Ttl
        {
            get
            {
                return (ExpirationDate - DateTime.UtcNow).TotalSeconds;
            }
        }

        public CacheObject(DateTime expirationDate, string value, double delta)
        {
            ExpirationDate = expirationDate;
            Value = value;
            Delta = delta;
        }
    }
}
