namespace BlogAPI.Models.dtos
{
    public class UpdateBlogPostDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int blogId { get; set; }
    }
}
