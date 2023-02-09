using Domain.Entites.Article;
using Domain.Entites.User;
using Infrastructure.ViewModels;

namespace Infrastructure.Services.Interfaces;

public interface IAdminService
{
    Task<AdminInfoViewModel> GetAdminInfoForPanel(Guid adminId);
    Task<List<User>> GetAllUsers(int take, int skip);
    Task<List<Article>> GetAllArticles(int take, int skip);
    Task<int> AllUsersCount();
    Task<int> AllArticlesCount();
}
