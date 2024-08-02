using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entites.Articles;
using Domain.Entites.Comments;

namespace Domain.Entites.Identity;

public sealed class User : Entity
{
    private User(
        Guid id,
        string? slug,
        string userName,
        string firstName,
        string lastName,
        string imageName,
        string? description) 
        : base(id, slug)
    {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        ImageName = imageName;
        Description = description;
    }

    public string UserName { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    [NotMapped] public string DisplayName
        => $"{FirstName} {LastName}";
    public string ImageName { get; private set; }
    public string? Description { get; private set; }

    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Article> Articles { get; set; }

    public static User Create(
        
        string userName,
        string firstName,
        string lastName,
        string imageName,
        string? description = null,
        string? slug = null)
    {
        return new User(
            Guid.NewGuid(),
            slug,
            userName,
            firstName,
            lastName,
            imageName,
            description);
    }
}

