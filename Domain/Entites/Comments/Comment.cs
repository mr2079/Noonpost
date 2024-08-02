using Domain.Entites.Articles;
using Domain.Entites.Identity;

namespace Domain.Entites.Comments;

public class Comment : Entity
{
    private Comment(
        Guid id,
        Guid userId,
        Guid articleId,
        Guid? parentId,
        string text,
        string? slug)
        : base(id, slug)
    {
        UserId = userId;
        ArticleId = articleId;
        ParentId = parentId;
        Text = text;
    }

    public Guid UserId { get; private set; }
    public Guid ArticleId { get; private set; }
    public Guid? ParentId { get; private set; }
    public string Text { get; private set; }

    public User User { get; set; }
    public Article Article { get; set; }
    public Comment? Parent { get; set; }
    public ICollection<Comment>? Replies { get; set; }

    public static Comment Create(
        Guid userId,
        Guid articleId,
        string text,
        Guid? parentId = null,
        string? slug = null)
    {
        return new Comment(
            Guid.NewGuid(),
            userId,
            articleId,
            parentId, 
            text, 
            slug);
    }
}
