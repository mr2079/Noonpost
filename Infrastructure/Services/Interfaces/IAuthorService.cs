using Infrastructure.ViewModels;

namespace Infrastructure.Services.Interfaces;

public interface IAuthorService
{
    Task<AuthorInfoViewModel> GetAuthorInfoAsync(Guid authorId, int take, int skip);
    Task<int> AuthorArticlesCount(Guid authorId);
}
