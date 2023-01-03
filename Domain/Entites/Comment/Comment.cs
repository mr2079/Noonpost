using System.ComponentModel.DataAnnotations;

namespace Domain.Entites.Comment;

public class Comment
{
    [Key]
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
    //public Guid ParentId { get; set; } = Guid.Empty; 
    [MaxLength(500)]
    public string Text { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; }
    public bool IsAccepted { get; set; } = false;

    // Navigation properties
    public User.User User { get; set; }
    public Article.Article Article { get; set; }
}
