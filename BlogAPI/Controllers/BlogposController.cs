using BlogAPI.Models;
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

        public object BloggerGetinformation([FromQuery]int id,Blogger blogger)
        {
            var connection = new MySqlConnection(connectionString);

            connection.Open();

            var sql = @"SELECT `@name`,`@email` FROM `blogger` WHERE id = `@id`";

            var cmd = new MySqlCommand(sql,connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", blogger.Name);
            cmd.Parameters.AddWithValue("@email", blogger.Email);

            var dataReader = cmd.ExecuteReader();


            connection.Close();

            return new { message = "Sikeres Lekérés"};
        }
    }
}
