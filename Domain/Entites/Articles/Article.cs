using Domain.Entites.Categories;
using Domain.Entites.Comments;
using Domain.Entites.Identity;

namespace Domain.Entites.Articles;

public sealed class Article : Entity
{
    private Article(
        Guid id,
        string? slug,
        Guid categoryId,
        Guid authorId,
        string title,
        string text,
        string imageName,
        string? tags) 
        : base(id, slug)
    {
        CategoryId = categoryId;
        AuthorId = authorId;
        Title = title;
        Text = text;
        ImageName = imageName;
        Tags = tags;
    }

    public Guid CategoryId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string ImageName { get; private set; }
    public string Title { get; private set; }
    public string Text { get; private set; }
    public string? Tags { get; private set; }
    public int View { get; set; }

    public Category Category { get; set; }
    public User User { get; set; }
    public ICollection<Comment>? Comments { get; set; }

    public static Article Create(
        Guid categoryId,
        Guid authorId,
        string title,
        string text,
        string imageName,
        string? tags = null,
        string? slug = null)
    {
        return new Article(
            Guid.NewGuid(),
            slug,
            categoryId,
            authorId,
            title,
            text,
            imageName,
            tags);
    }
}
