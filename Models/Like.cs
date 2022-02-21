namespace CrystalApi.Models;

public class Like
{
    public Like()
    {
        Id = Guid.NewGuid().ToString();
    }
    public string Id { get; set; }
    public Post Post { get; set; }
    public string PostId { get; set; }
}