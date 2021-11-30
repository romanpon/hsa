using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace L8.MySQL
{
    public static class DbInit
    {
        private static Random _rnd = new Random();
        
        public static void Init(string connectiongString)
        {
            CreateTable(connectiongString);
            Seed(connectiongString);
        }

        public static void CreateTable(string connectiongString)
        {
            using var con = new MySqlConnection(connectiongString);
            con.Open();
            var query = @"CREATE TABLE if not exists `users` (
                `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
                `userName` varchar(25) DEFAULT NULL,
                `bio` varchar(250) DEFAULT NULL,
                `birthDate` DATE,
                PRIMARY KEY (`id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8;";
            using var cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }

        public static void Seed(string connectiongString)
        {
            using var con = new MySqlConnection(connectiongString);
            con.Open();
            for (int i = 0; i < 40 * 1000 * 1000;)
            {
                using var cmd = new MySqlCommand();
                var sql = "INSERT INTO users (userName, bio, birthDate) VALUES ";

                for (int k = 0; k < 10 * 1000 && i <= 40 * 1000 * 1000; k++, i++)
                {
                    var userName = RandomString(25);
                    var bio = RandomString(250);
                    var bday = RandomDate();

                    sql += "(@userName" + i + ", @bio" + i + ", @bday" + i + "),";
                    cmd.Parameters.Add(new MySqlParameter("userName" + i, userName));
                    cmd.Parameters.Add(new MySqlParameter("bio" + i, bio));
                    cmd.Parameters.Add(new MySqlParameter("bday" + i, bday.ToString("yyyy-MM-dd H:mm:ss")));
                }

                sql = sql.TrimEnd(',');
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        public static string RandomString(int n)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var str = new string(Enumerable.Repeat(chars, n)
                .Select(s => s[_rnd.Next(s.Length)]).ToArray());
            return str;
        }

        public static DateTime RandomDate()
        {
            var d = _rnd.Next(1, 28);
            var m = _rnd.Next(1, 12);
            var year = _rnd.Next(1700, 2021);
            return new DateTime(year, m, d);
        }
    }
}
