using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "نام")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    [MinLength(2, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    [MinLength(2, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public string Mobile { get; set; } = string.Empty;

    [Display(Name = "کلمه عبور")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "تکرار کلمه عبور")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "{0} با {1} برابر نیست")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public string RePassword { get; set; } = string.Empty;

    [Display(Name = "موافقت با شرایط و ضوابط")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public bool IsRulesAccepted { get; set; }
}

public class LoginViewModel
{
    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public string Mobile { get; set; } = string.Empty;

    [Display(Name = "کلمه عبور")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "مرا به خاطر بسپار")]
    public bool RememberMe { get; set; }
}
