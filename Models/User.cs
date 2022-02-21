using System.ComponentModel.DataAnnotations;

namespace CrystalApi.Models;

public class User
{
    public User()
    {
        Id = Guid.NewGuid().ToString();
    }
    public string Id { get; set; }
    public string FirstName { get; set; }
    public  string LastName { get; set; }
    public  string Email { get; set; }
    public  string Password { get; set; }
    public string ProfilePhoto { get; set; } = "none.png";
    public bool Confirmed { get; set; } = false;
    public bool Verified { get; set; } = false;
    public List<Auth> Auths { get; set; } = new List<Auth>();
    public List<Invite> Invites { get; set; } = new List<Invite>();
    public List<Post> Posts { get; set; } = new List<Post>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public  DateTime UpdatedAt { get; set; } = DateTime.Now;
}