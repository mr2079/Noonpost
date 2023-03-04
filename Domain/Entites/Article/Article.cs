using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entites.Article;

public class Article : BaseEntity
{
    public Guid CategoryId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? ImagesGuid { get; set; }
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
    [ForeignKey(nameof(CategoryId))]
    public virtual Category.Category? Category { get; set; }
    [ForeignKey(nameof(AuthorId))]
    public virtual User.User User { get; set; } = null!;
    public virtual List<Comment.Comment>? Comments { get; set; }
}
