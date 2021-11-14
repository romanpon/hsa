using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ABTest.Controllers
{
    [ApiController]
    [Route("insert-test")]
    public class InsertTestController : ControllerBase
    {
        private readonly ILogger<InsertTestController> _logger;

        public InsertTestController(ILogger<InsertTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public bool Get()
        {
            var queries = new List<string>();
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for (int i = 0; i < 100; i++)
            {
                var str = new string(Enumerable.Repeat(chars, 1000)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                queries.Add($"('{str}')");
            }

            var insertQuery = $"INSERT INTO TestTable (message) VALUES {string.Join(",", queries)};";
            var connectionString = "Server = DESKTOP-G3EQEMK\\SQLEXPRESS; Trusted_Connection = True; MultipleActiveResultSets = true; Integrated Security=SSPI; DataBase = HSA";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = connection.Execute(insertQuery);
            }

            return true;
        }
    }
}
