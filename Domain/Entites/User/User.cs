using System.ComponentModel.DataAnnotations;

namespace Domain.Entites.User;

public class User
{
    [Key]
    public Guid UserId { get; set; }
    [MaxLength(255)]
    public string? ImageName { get; set; } = "Default.jpg";
    [MaxLength(255)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(255)]
    public string LastName { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Mobile { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? Email { get; set; } = null;
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;
    [MaxLength(600)]
    public string? Description { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Role { get; set; } = "author";
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; }

    // Navigation properties
    public List<Comment.Comment>? Comments { get; set; }
    public List<Article.Article>? Articles { get; set; }
}

