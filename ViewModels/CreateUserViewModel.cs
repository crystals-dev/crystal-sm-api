using System.ComponentModel.DataAnnotations;

namespace CrystalApi.ViewModels;

public class CreateUserViewModel : ResendViewModel
{
    [Required]
    public string FirstName { get; set; } = "";
    [Required]
    public  string LastName { get; set; } = "";
    [Required]
    public  string Password { get; set; } = "";
}