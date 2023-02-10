using Application.Context;
using Domain.Entites.Article;
using Infrastructure.Generator;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Services;

public class ArticleService : IArticleService
{
    private readonly NoonpostDbContext _context;

    public ArticleService(NoonpostDbContext context)
    {
        _context = context;
    }

    public async Task<int> ArticleCommentsCount(Guid articleId)
        => await _context.Comments
        .CountAsync(c => Equals(c.ArticleId, articleId) && Equals(c.ParentId, null));

    public async Task<bool> IsExistsArticle(Guid articleId) => await _context.Articles.AnyAsync(a => Guid.Equals(a.Id, articleId));

    public async Task<Article> GetArticleByIdAsync(Guid articleId) => await _context.Articles.FindAsync(articleId);

    public async Task<Article> GetArticleForShowAsync(Guid articleId, int take, int skip)
    {
        var model = await _context.Articles
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == articleId);

        model.View += 1;
        _context.Articles.Update(model);
        await _context.SaveChangesAsync();

        var comments = await _context.Comments
            .AsNoTrackingWithIdentityResolution()
            .Include(c => c.User)
            .Include(c => c.Replies.OrderByDescending(r => r.CreateDate))
            .ThenInclude(r => r.User)
            .Where(c => c.ArticleId == articleId)
            .ToListAsync();

        model.Comments = comments
            .Where(c => c.ParentId == null)
            .OrderByDescending(c => c.CreateDate)
            .AsParallel()
            .Skip(skip)
            .Take(take)
            .ToList();

        return model;
    }

    public async Task<bool> CreateArticleAsync(CreateArticleViewModel articleInfo, IFormFile image)
    {
        var article = new Article()
        {
            AuthorId = articleInfo.AuthorId,
            Title = articleInfo.Title,
            Text = articleInfo.Text,
            Tags = articleInfo.Tags,
            CreateDate = DateTime.Now,
            ImageName = NameGenerator.Generate() + Path.GetExtension(image.FileName)
        };

        try
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\images\\articles", article.ImageName);
            using (var fs = new FileStream(imagePath, FileMode.Create)) image.CopyTo(fs);

            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<EditArticleViewModel> EditArticleAsync(Guid articleId)
    {
        return await _context.Articles
            .Select(a => new EditArticleViewModel()
            {
                ArticleId = a.Id,
                AuthorId = a.AuthorId,
                ImageName = a.ImageName,
                Title = a.Title,
                Text = a.Text,
                Tags = a.Tags,
            })
            .FirstOrDefaultAsync(a => a.ArticleId == articleId);
    }

    public async Task<bool> UpdateArticleAsync(EditArticleViewModel edit, IFormFile? newArticleImg)
    {
        try
        {
            var article = await _context.Articles.FindAsync(edit.ArticleId);
            article.Title = edit.Title;
            article.Tags = edit.Tags;
            article.UpdateDate = DateTime.Now;
            if (newArticleImg != null)
            {
                var oldImagePath = Path
                    .Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\articles", article.ImageName);
                if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);

                article.ImageName = NameGenerator.Generate() + Path.GetExtension(newArticleImg.FileName);
                var newImagePath = Path
                    .Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\articles", article.ImageName);
                using (var fs = new FileStream(newImagePath, FileMode.Create)) newArticleImg.CopyTo(fs);
            }
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteArticleAsync(Article article)
    {
        try
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<int> ArticlesCountAsync()
        => await _context.Articles.CountAsync();

    public async Task<List<Article>> GetArticlesForSlider()
        => await _context.Articles
            .Where(a => a.IsAccepted)
            .OrderByDescending(a => a.View)
            .Take(5)
            .ToListAsync();

    public async Task<List<Article>> GetArticlesForIndex(int take, int skip)
        => await _context.Articles
            .Include(a => a.User)
            .Where(a => a.IsAccepted)
            .OrderByDescending(a => a.CreateDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

    public async Task<List<Article>> GetArticlesByFilter(string filter, int take, int skip)
        => await _context.Articles
            .Include(a => a.User)
            .Where(a => (a.Title.Contains(filter) || a.Tags.Contains(filter)) && a.IsAccepted)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
}

