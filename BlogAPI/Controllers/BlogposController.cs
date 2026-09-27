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
        public string connectionString = "server=localhost;port=3307;uid=root;password=;database=blog;";

        [HttpGet]
        public object GetBlogs()
        {
            List<Blogpost> bloggers = new List<Blogpost>();
            var connection = new MySqlConnection(connectionString);

            connection.Open();

            string sql = "SELECT * FROM blogpost";
            var cmd = new MySqlCommand(sql, connection);
            var data = cmd.ExecuteReader();
            while (data.Read())
            {
                Blogpost bloggERS = new Blogpost()
                {
                    Id = data.GetInt32(0),
                    Title = data.GetString(1),
                    Content = data.GetString(2),
                    postTime = data.GetDateTime(3),
                    updateTime = data.GetDateTime(4),
                    blogId = data.GetInt32(5),

                };
                bloggers.Add(bloggERS);
            }

            connection.Close();

            return bloggers;
        }
        [HttpPost]

        public object addNewblog([FromBody] AddNewBlogPostDTO addNewBlogPostDTO)
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

        public object DeleteBlog(int id)
        {

            var connection = new MySqlConnection(connectionString);

            connection.Open();

            var sql = @"DELETE FROM `blogpost` WHERE `Id` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new { message = "Sikeres törlés" };
        }

        [HttpPut]

        public object UpdateBlog([FromQuery] int id, UpdateBlogPostDTO updateBlogPostDTO)
        {
            var connection = new MySqlConnection(connectionString);

            connection.Open();

            var sql = @"UPDATE `blogpost` SET `Title`=@title,`Content`=@content,`blogId`=@blogId WHERE `Id` = @id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@title", updateBlogPostDTO.Title);
            cmd.Parameters.AddWithValue("@content", updateBlogPostDTO.Content);
            cmd.Parameters.AddWithValue("@blogId", updateBlogPostDTO.blogId);

            cmd.ExecuteNonQuery();
            connection.Close();

            return new { message = "Siekres frissítés", resault = updateBlogPostDTO };
        }

        [HttpGet("Allrekord")]

        public object GetAllpost()
        {
            long recordCount = 0;

            var connection = new MySqlConnection(connectionString);

            connection.Open();

            var sql = @"SELECT COUNT(`Id`) FROM `blogpost` WHERE 1;";

            var cmd = new MySqlCommand(sql, connection);

            var result = cmd.ExecuteScalar();

            if (result != null)
            {
                recordCount = Convert.ToInt64(result);
            }
            return new { result = recordCount };
        }

        [HttpGet("GetPostByName")]
        public object GetAllPost(string name)
        {

            List<Blogpost> blogposts = new List<Blogpost>();
            var connection = new MySqlConnection(connectionString);

            var sql = @"SELECT blogpost.Title,blogpost.Content FROM `blogger` INNER JOIN blogpost ON blogger.Id = blogpost.blogId WHERE `Name` = @name;";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@name", name);

            connection.Open();

            var datareader = cmd.ExecuteReader();
            object? data = null;
            while (datareader.Read())
            {
                var blogpostM = new Blogpost
                {
                    Title = datareader.GetString(0),
                    Content = datareader.GetString(1),
                };
                blogposts.Add(blogpostM);
                if (blogpostM != null)
                {
                    data = new { message = "Sikeres Lekérdezés", blogposts };
                }
            }
            connection.Close();
            return data;
        }

        [HttpGet("BloggersPost")]

        public object GetAllPostById(int id)
        {
            long recordCount = 0;

            var connection = new MySqlConnection(connectionString);

            var sql = @"SELECT COUNT(*) FROM `blogger` INNER JOIN blogpost ON blogger.Id = blogpost.blogId WHERE `blogId` = @id;";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();

            var result = cmd.ExecuteScalar();

            recordCount = Convert.ToInt32(result);

            connection.Close();
            return new { result = recordCount };
        }
    }
}
