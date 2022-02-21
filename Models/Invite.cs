namespace CrystalApi.Models;

public class Invite
{
    public Invite()
    {
        Id = Guid.NewGuid().ToString();
    }
    public string Id { get; set; }
    public User User { get; set; }
    public string FromId { get; set; }
    public  string ToId { get; set; }
    public  DateTime CreatedAt { get; set; } = DateTime.Now;
}