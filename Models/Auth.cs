namespace CrystalApi.Models;

public class Auth
{
    public Auth()
    {
        Id = Guid.NewGuid().ToString();
    }
    public string Id { get; set; }
    public User User { get; set; }
    public string UserId { get; set; }
    public int Code { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}