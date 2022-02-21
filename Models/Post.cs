namespace CrystalApi.Models;

public class Post
{
    public Post()
    {
        Id = Guid.NewGuid().ToString();
    }
    public string Id { get; set; }
    public string Content { get; set; } = "";
    public User User { get; set; }
    public string UserId { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<Like> Likes { get; set; } = new List<Like>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}