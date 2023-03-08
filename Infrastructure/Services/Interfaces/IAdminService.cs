using Domain.Entites.Article;
using Domain.Entites.Category;
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

    Task<bool> DeleteUser(Guid userId);
    Task<bool> EditUserRole(Guid userId, string role);
    Task<bool> DeleteArticle(Guid articleId);
    Task<bool> DeclineArticle(Guid articleId);
    Task<bool> AcceptArticle(Guid articleId);
    Task<bool> AcceptComment(Guid commentId);
    Task<bool> DeleteComment(Guid commentId);
    Task<Tuple<List<ArticlesWithNewCommentViewModel>, int>> GetArticlesWithNewComments();
    Task<AdminDashboardViewModel> GetAdminDashboardInfo();
    Task<List<CategoryViewModel>> GetAllCategoriesAsync();
    Task<int> AllCategoriesCount();
    Task<bool> CreateCategoryAsync(string title);
    Task<bool> UpdateCategoryAsync(Guid Id, string title);
    Task<bool> DeleteCategoryAsync(Guid Id);
}
