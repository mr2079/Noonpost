using System.ComponentModel.DataAnnotations;

namespace Domain.Entites;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; }
}
