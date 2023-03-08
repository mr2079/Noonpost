using Infrastructure.ViewModels;

namespace Infrastructure.Services.Interfaces;

public interface IAuthorService
{
    Task<AuthorInfoViewModel> GetAuthorInfoAsync(long authorCId, int take, int skip);
    Task<int> AuthorArticlesCount(long authorCId);
}
