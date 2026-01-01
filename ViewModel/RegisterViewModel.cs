using System;
using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.ViewModel;

public class RegisterViewModel
{
    [Display(Name = "Email Adress")]
    [Required(ErrorMessage ="Email is required, please")]
    public string Email { get; set; }
    [Required(ErrorMessage ="Password is required, please")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Display(Name = "Confirm Password")]
    [Required(ErrorMessage = "Confirm Password is required, please")]
    [DataType(DataType.Password)]
    [Compare("Password",ErrorMessage ="Password doesn't match")]
    public string ConfirmPassword { get; set; }
}
