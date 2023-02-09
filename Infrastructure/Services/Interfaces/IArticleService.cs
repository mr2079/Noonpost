using Domain.Entites.Article;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Interfaces;

public interface IArticleService
{
    Task<int> ArticlesCountAsync();
    Task<List<Article>> GetArticlesForSlider();
    Task<List<Article>> GetArticlesForIndex(int take, int skip);
    Task<List<Article>> GetArticlesByFilter(string filter, int take, int skip);

    Task<bool> IsExistsArticle(Guid articleId);
    Task<Article> GetArticleByIdAsync(Guid articleId);
    Task<Article> GetArticleForShowAsync(Guid articleId, int take, int skip);
    Task<bool> CreateArticleAsync(CreateArticleViewModel articleInfo, IFormFile image);
    Task<EditArticleViewModel> EditArticleAsync(Guid articleId);
    Task<bool> UpdateArticleAsync(EditArticleViewModel edit, IFormFile? newArticleImg);
    Task<bool> DeleteArticleAsync(Article article);
}
