using Domain.Entites.Article;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.ViewModels;

public class UserPanelInfoViewModel
{
    public Guid UserId { get; set; }

    [Display(Name = "نام")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    [MinLength(2, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    [MinLength(2, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "شماره موبایل")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public string Mobile { get; set; } = string.Empty;

    [Display(Name = "آدرس ایمیل")]
    [EmailAddress(ErrorMessage = "{0} وارد شده، معتبر نیست")]
    public string? Email { get; set; } 

    [Display(Name = "کلمه عبور جدید")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
    public string? Password { get; set; }

    [Display(Name = "تکرار کلمه عبور جدید")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "{0} با {1} برابر نیست")]
    public string? RePassword { get; set; }

    [Display(Name = "نام تصویر کاربر")]
    public string? ImageName { get; set; }

    [Display(Name = "تصویر کاربر")]
    public IFormFile? Image { get; set; }

    [Display(Name = "درباره من")]
    public string? Description { get; set; }

    public ICollection<Article>? Articles { get; set; }
}
