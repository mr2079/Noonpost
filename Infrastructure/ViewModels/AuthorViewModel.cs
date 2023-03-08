using Domain.Entites.Article;

namespace Infrastructure.ViewModels;

public class AuthorInfoViewModel
{
    public long CId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Article>? Articles { get; set; }
}
