using Domain.Entites.Article;
using Domain.Entites.Category;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Interfaces;

public interface IArticleService
{
    Task<int> ArticlesCountAsync();
    Task<List<Article>> GetArticlesForSlider();
    Task<List<Article>> GetArticlesForIndex(int take, int skip);

    Task<bool> IsExistsArticle(Guid articleId);
    Task<Article> GetArticleByIdAsync(Guid articleId);
    Task<Article> GetArticleForShowAsync(long articleCId, int take, int skip);
    Task<bool> CreateArticleAsync(CreateArticleViewModel articleInfo, IFormFile image);
    Task<EditArticleViewModel> EditArticleAsync(Guid articleId);
    Task<bool> UpdateArticleAsync(EditArticleViewModel edit, IFormFile? newArticleImg);
    Task<bool> DeleteArticleAsync(Article article);
    Task<int> ArticleCommentsCount(long articleCId);
    Task<int> ArticleAcceptedCommentsCount(long articleCId);
    Task<Tuple<string, string>> SaveUploadedArticleImage(IFormFile image);
    Task<bool> AddArticleImage(Guid articleImageGuid, string imageName);
    Task<List<Category>> GetAllCategoriesAsync();
    Task<List<Category>> GetCategoriesForNavBarAsync();
    Task<Tuple<List<Article>, int>> GetArticlesByFilter(string filter, int take, int skip);
    Task<Tuple<List<Article>, int>> GetArticlesByCategoryIdAsync(long categoryCId, int take, int skip);
    Task<Tuple<long, string>> GetArticleCIdByArticleId(Guid articleId);
}
