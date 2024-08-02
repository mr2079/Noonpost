using System.ComponentModel.DataAnnotations;

namespace Domain.Entites;

public abstract class Entity
{
    protected Entity(
        Guid id,
        string? slug)
    {
        Id = id;
        Slug = slug;
    }

    public Guid Id { get; init; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }

    public string? Slug { get; set; }
    public bool IsAccepted { get; set; }
    public bool IsDeleted { get; set; }
}
