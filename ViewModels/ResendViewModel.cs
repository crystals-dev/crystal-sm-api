using System.ComponentModel.DataAnnotations;

namespace CrystalApi.ViewModels;

public class ResendViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}