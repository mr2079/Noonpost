using Application.Context;
using Domain.Entites.Article;
using Domain.Entites.User;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AdminService : IAdminService
{
    private readonly NoonpostDbContext _context;
    private readonly IBaseService _baseService;

    public AdminService(NoonpostDbContext context, IBaseService baseService)
    {
        _context = context;
        _baseService = baseService;
    }

    public async Task<int> AllUsersCount()
        => await _context.Users.CountAsync();

    public async Task<int> AllArticlesCount()
        => await _context.Articles.CountAsync();

    public async Task<AdminInfoViewModel> GetAdminInfoForPanel(Guid adminId)
        => await _context.Users
            .Where(u => Equals(u.Id, adminId))
            .Select(u => new AdminInfoViewModel
            {
                FullName = $"{u.FirstName} {u.LastName}",
                ImageName = u.ImageName
            })
            .FirstOrDefaultAsync();


    public async Task<List<User>> GetAllUsers(int take, int skip)
        => await _context.Users
            .OrderByDescending(u => u.CreateDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();


    public async Task<bool> DeleteUser(Guid userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> EditUserRole(Guid userId, string role)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            if (user.Role == role) return true;
            user.Role = role;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<List<Article>> GetAllArticles(int take, int skip)
        => await _context.Articles
            .Include(a => a.User)
            .OrderBy(a => !a.IsAccepted)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

    public async Task<bool> DeleteArticle(Guid articleId)
    {
        try
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null) return false;
            await _baseService.DeleteArticleImageFile(article.ImageName);
            if (article.ImagesGuid != null)
            {
                var articleImages = await _context.ArticleImages
                    .Where(ai => Equals(ai.ArticleImageGuid, article.ImagesGuid))
                    .ToListAsync();

                foreach (var image in articleImages)
                {
                    await _baseService.DeleteArticleImageFile(image.ImageName);
                    _context.ArticleImages.Remove(image);
                }    
            }
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> AcceptArticle(Guid articleId)
    {
        try
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null) return false;
            article.IsAccepted = true;
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> DeclineArticle(Guid articleId)
    {
        try
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null) return false;
            article.IsAccepted = false;
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> AcceptComment(Guid commentId)
    {
        try
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return false;
            comment.IsAccepted = true;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> DeleteComment(Guid commentId)
    {
        try
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return false;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<Tuple<List<ArticlesWithNewCommentViewModel>, int>> GetArticlesWithNewComments()
    {
        var articles = await _context.Articles
            .Where(a => a.IsAccepted)
            .Include(a => a.Comments)
            .Select(a => new ArticlesWithNewCommentViewModel()
            {
                ArticleId = a.Id,
                ArticleTitle = a.Title,
                ArticleImageName = a.ImageName,
                NewCommentsCount = a.Comments.Count(c => !c.IsAccepted)
            })
            .ToListAsync();

        foreach (var article in articles.ToList())
        {
            if (article.NewCommentsCount == 0) articles.Remove(article);
        }

        int articlesCount = articles.Count();

        return Tuple.Create(articles, articlesCount);
    }
}
