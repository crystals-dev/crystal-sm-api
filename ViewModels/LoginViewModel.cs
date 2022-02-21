using System.ComponentModel.DataAnnotations;

namespace CrystalApi.ViewModels;

public class LoginViewModel : ResendViewModel
{
    [Required]
    public string Password { get; set; }
}