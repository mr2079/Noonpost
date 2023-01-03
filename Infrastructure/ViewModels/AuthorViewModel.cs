using Domain.Entites.Article;

namespace Infrastructure.ViewModels;

public class AuthorInfoViewModel
{
    public Guid AuthorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Article>? Articles { get; set; }
}
