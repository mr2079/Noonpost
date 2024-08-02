using Domain.Entites.Articles;

namespace Domain.Entites.Categories;

public sealed class Category : Entity
{
    private Category(
        Guid id,
        string? slug,
        string title) 
        : base(id, slug)
    {
        Title = title;
    }

    public string Title { get; private set; }

    public ICollection<Article>? Articles { get; set; }

    public static Category Create(
        string title,
        string? slug = null)
    {
        return new Category(
            Guid.NewGuid(),
            slug,
            title);
    }
}
