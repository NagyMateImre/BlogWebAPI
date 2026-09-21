using BlogAPI.Models;
using BlogAPI.Models.dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogposController : ControllerBase
    {
        public string connectionString = "server=localhost;uid=root;password=;database=blog;";

        [HttpGet]

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
                    data = new { message = "Sikeres Lekérdezés", blogger.Name,blogger.Email };
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
