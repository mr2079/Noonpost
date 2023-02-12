using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace Domain.Entites.Article;

public class ArticleImage : BaseEntity
{
    public Guid ArticleImageGuid { get; set; }
    public string ImageName { get; set; } = string.Empty;
}
