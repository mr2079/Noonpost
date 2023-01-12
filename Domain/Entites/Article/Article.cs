using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entites.Article;

public class Article : BaseEntity
{
    public Guid AuthorId { get; set; }
    [MaxLength(255)]
    public string ImageName { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? Tags { get; set; }
    public bool IsAccepted { get; set; } = false;
    public int View { get; set; } = 0;

    // Navigation properties
    [ForeignKey(nameof(AuthorId))]
    public User.User User { get; set; }
    public List<Comment.Comment>? Comments { get; set; }
}
