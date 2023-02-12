namespace Infrastructure.ViewModels;

public class AdminInfoViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
}

public class ArticlesWithNewCommentViewModel
{
    public Guid ArticleId { get; set; }
    public string ArticleTitle { get; set; } = string.Empty;
    public string ArticleImageName { get; set; } = string.Empty;
    public int NewCommentsCount { get; set; }
}