using BlogAPI.Models;
using BlogAPI.Models.dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MySqlConnector;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public string connectionString = "server=localhost;uid=root;password=;database=blog;";
        [HttpGet]
        public object GetBloggers()
        {
            List<Blogger> bloggers = new List<Blogger>();
            var connection = new MySqlConnection(connectionString);

            connection.Open();

            string sql = "SELECT * FROM blogger";
            var cmd = new MySqlCommand(sql, connection);
            var data = cmd.ExecuteReader();
            while (data.Read())
            {
                var bloggERS = new Blogger()
                {
                    Id = data.GetInt32("id"),
                    Name = data.GetString("name"),
                    Email = data.GetString("email"),
                    Age = data.GetInt32("age"),
                    Password = data.GetString("password"),
                    RegistrationTime = data.GetDateTime("registrationtime")
                };
                bloggers.Add(bloggERS);
            }

            connection.Close();

            return bloggers;
        }

        [HttpPost]

        public object AddNewBloggers([FromBody]AddNewBloggerDTO NewBloggerDTO)
        {
            var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"INSERT INTO `blogger`(`Name`, `Email`, `Age`, `Password`, `RegistrationTime`) VALUES (@name,@email,@age,@password,@registrationTime)";

            var cmd = new MySqlCommand(sql,connection);


            cmd.Parameters.AddWithValue("@name", NewBloggerDTO.Name);
            cmd.Parameters.AddWithValue("@email", NewBloggerDTO.Email);
            cmd.Parameters.AddWithValue("@age", NewBloggerDTO.Age);
            cmd.Parameters.AddWithValue("@password", NewBloggerDTO.Password);
            cmd.Parameters.AddWithValue("@registrationTime", DateTime.Now);

            cmd.ExecuteNonQuery();

            connection.Close();
            return new { message = "Sikeres Felvétel", result = NewBloggerDTO};
        }

        [HttpDelete]

        public object DeleteBloggers(int id){

            var connection = new MySqlConnection(connectionString);

            connection.Open();

            var sql = @"DELETE FROM `blogger` WHERE `id` = @id";

            var cmd = new MySqlCommand(sql,connection);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres törlés" };
        }

        [HttpPut]

        public object UpdateBloggers([FromQuery]int id,UpdateBloggerDTO updateBloggerDTO)
        {
            var connection = new MySqlConnection(connectionString);

            connection.Open();

            var sql = @"UPDATE `blogger` SET `Name`=@name,`Email`=@email,`Age`=@age,`Password`=@password WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id",id);
            cmd.Parameters.AddWithValue("@name", updateBloggerDTO.Name);
            cmd.Parameters.AddWithValue("@email", updateBloggerDTO.Email);
            cmd.Parameters.AddWithValue("@age", updateBloggerDTO.Age);
            cmd.Parameters.AddWithValue("@password", updateBloggerDTO.Password);

            cmd.ExecuteNonQuery();
            connection.Close();

            return new { message = "Siekres frissítés", resault = updateBloggerDTO };
        }

        [HttpGet("byId")]

        public object GetBloggerByID(int id)
        {
            var connection = new MySqlConnection(connectionString);

            connection.Open();

            var sql = @"SELECT * FROM `blogger` WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();
            object? data = null;
            if (datareader.Read() == true)
            {
                var blogger = new Blogger
                {
                    Id = datareader.GetInt32(0),
                    Name = datareader.GetString(1),
                    Email = datareader.GetString(2),
                    Age = datareader.GetInt32(3),
                    Password = datareader.GetString(4),
                    RegistrationTime = datareader.GetDateTime(5)
                };
                if (blogger != null)
                {
                    data = new { message = "Sikeres Lekérdezés", result = blogger };
                }
            }
            else
            {
                data = new { message = "Sikertelen Lekérés", result = "" };
            }


            connection.Close();
            return data;
        }

        [HttpGet("NameEmailById")]

        public object BloggerGetinformation(int id)
        {
            var connection = new MySqlConnection(connectionString);

            connection.Open();

            var sql = @"SELECT `Name`,`Email` FROM `blogger` WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();
            object? data = null;
            if (datareader.Read() == true)
            {
                var blogger = new Blogger
                {
                    Name = datareader.GetString(0),
                    Email = datareader.GetString(1),
                };
                if (blogger != null)
                {
                    data = new { message = "Sikeres Lekérdezés", blogger.Name, blogger.Email };
                }
            }
            else
            {
                data = new { message = "Sikertelen Lekérés", result = "" };
            }
            connection.Close();
            return data;
        }
    }
}
