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
        public object GetBloggers()
        {
            List<Blogpost> bloggers = new List<Blogpost>();
            var connection = new MySqlConnection(connectionString);

            connection.Open();

            string sql = "SELECT * FROM blogger";
            var cmd = new MySqlCommand(sql, connection);
            var data = cmd.ExecuteReader();
            while (data.Read())
            {
                var bloggERS = new Blogpost()
                {
                    Id = data.GetInt32("Id"),
                    Title = data.GetString("Title"),
                    Content = data.GetString("Content"),
                    postTime = data.GetDateTime("postTime"),
                    updateTime = data.GetDateTime("updateTime"),
                    blogid = data.GetInt32("blogId"),

                };
                bloggers.Add(bloggERS);
            }

            connection.Close();

            return bloggers;
        }

        [HttpPost]

        public object AddNewBloggers([FromBody] AddNewBlogPostDTO addNewBlogPostDTO)
        {
            var connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = @"INSERT INTO `blogpost`( `Title`, `Content`, `postTime`, `updateTime`, `blogId`) VALUES (@Title,@Content,@postTime,@updateTime,@blogId)";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Title", addNewBlogPostDTO.Title);
            cmd.Parameters.AddWithValue("@Content", addNewBlogPostDTO.Content);
            cmd.Parameters.AddWithValue("@postTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@blogId", addNewBlogPostDTO.blogId);

            cmd.ExecuteNonQuery();

            connection.Close();
            return new { message = "Sikeres Felvétel", result = addNewBlogPostDTO };
        }

        [HttpDelete]

        public object DeleteBloggers(int id)
        {

            var connection = new MySqlConnection(connectionString);

            connection.Open();

            var sql = @"DELETE FROM `blogger` WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres törlés" };
        }

        [HttpPut]

        public object UpdateBloggers([FromQuery] int id, UpdateBloggerDTO updateBloggerDTO)
        {
            var connection = new MySqlConnection(connectionString);

            connection.Open();

            var sql = @"UPDATE `blogger` SET `Name`=@name,`Email`=@email,`Age`=@age,`Password`=@password WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", updateBloggerDTO.Name);
            cmd.Parameters.AddWithValue("@email", updateBloggerDTO.Email);
            cmd.Parameters.AddWithValue("@age", updateBloggerDTO.Age);
            cmd.Parameters.AddWithValue("@password", updateBloggerDTO.Password);

            cmd.ExecuteNonQuery();
            connection.Close();

            return new { message = "Siekres frissítés", resault = updateBloggerDTO };
        }
    }
}
