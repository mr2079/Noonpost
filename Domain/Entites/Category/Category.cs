using System.ComponentModel.DataAnnotations;

namespace Domain.Entites.Category;

public class Category : BaseEntity
{
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    public virtual List<Article.Article>? Articles { get; set; }
}
