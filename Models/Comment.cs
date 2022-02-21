namespace CrystalApi.Models;

public class Comment
{
    public Comment()
    {
        Id = Guid.NewGuid().ToString();
    }
    public string Id { get; set; }
    public string Content { get; set; }
    public Post Post { get; set; }
    public string PostId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}