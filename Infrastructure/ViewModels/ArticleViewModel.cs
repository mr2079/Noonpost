using Domain.Entites.Category;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.ViewModels;

public class CreateArticleViewModel
{
    public Guid AuthorId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? ArticleImageGuid { get; set; }

    [Display(Name = "عنوان مقاله")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "متن مقاله")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public string Text { get; set; } = string.Empty;

    [Display(Name = "تصویر مقاله")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public IFormFile Image { get; set; }

    [Display(Name = "برچسب ها")]
    public string? Tags { get; set; }

    public List<Category> Categories { get; set; } = new();
}

public class EditArticleViewModel
{
    public Guid ArticleId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid AuthorId { get; set; }

    [Display(Name = "عنوان مقاله")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "متن مقاله")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public string Text { get; set; } = string.Empty;

    [Display(Name = "تصویر مقاله")]
    public IFormFile? Image { get; set; }

    [Display(Name = "نام تصویر مقاله")]
    public string? ImageName { get; set; } = string.Empty;

    [Display(Name = "برچسب ها")]
    public string? Tags { get; set; }

    public List<Category> Categories { get; set; } = new();
}

public class LatestArticleViewModel
{
    public long CId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
}