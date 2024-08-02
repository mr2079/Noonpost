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
        string? firstName,
        string? lastName,
        string? email,
        string? phoneNumber,
        string imageName,
        string? description,
        string passwordHash) 
        : base(id, slug)
    {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        ImageName = imageName;
        Description = description;
    }

    public string UserName { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string ImageName { get; private set; }
    public string? Description { get; private set; }

    public string PasswordHash { get; private set; }

    [NotMapped]
    public string? DisplayName
        => string.IsNullOrWhiteSpace(FirstName)
           && string.IsNullOrWhiteSpace(LastName)
            ? $"{FirstName} {LastName}"
            : null;

    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Article> Articles { get; set; }

    public static User Create(
        string? slug,
        string userName,
        string imageName,
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        string? phoneNumber = null,
        string? description = null)
    {
        return new User(
            Guid.NewGuid(),
            slug,
            userName,
            firstName,
            lastName,
            email,
            phoneNumber,
            imageName,
            description,
            string.Empty);
    }
}

