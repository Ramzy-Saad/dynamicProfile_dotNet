using System;
using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.ViewModel;

public class LoginViewModel
{
    [Display(Name = "Email Adress")]
    [Required(ErrorMessage ="Email is required, please")]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
