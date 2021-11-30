using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace L8.MySQL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly string _connectionString;

        public DataController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnection");
        }

        [HttpPost]
        public void Insert()
        {
            using var con = new MySqlConnection(_connectionString);
            con.Open();
            using var cmd = new MySqlCommand();
            var sql = "INSERT INTO users (userName, bio, birthDate) VALUES ";

            var userName = DbInit.RandomString(25);
            var bio = DbInit.RandomString(250);
            var bday = DbInit.RandomDate();

            sql += "(@userName, @bio, @bday)";
            cmd.Parameters.Add(new MySqlParameter("userName", userName));
            cmd.Parameters.Add(new MySqlParameter("bio" , bio));
            cmd.Parameters.Add(new MySqlParameter("bday", bday.ToString("yyyy-MM-dd H:mm:ss")));

            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
    }
}
