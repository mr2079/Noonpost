using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites.Comment;

public class Comment
{
    [Key]
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
    public Guid? ParentId { get; set; }
    [MaxLength(500)]
    public string Text { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; }
    public bool IsAccepted { get; set; } = false;

    // Navigation properties
    public User.User User { get; set; }
    public Article.Article Article { get; set; }
    [ForeignKey(nameof(ParentId))]
    public Comment? Parent { get; set; }
    public List<Comment>? Replies { get; set; }
}
