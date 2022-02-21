using System.ComponentModel.DataAnnotations;

namespace CrystalApi.ViewModels;

public class VerifyViewModel : ResendViewModel
{
    [Required]
    public int Code { get; set; }
}