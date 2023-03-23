using Application.Context;
using Domain.Entites.Article;
using Domain.Entites.Category;
using Infrastructure.Converter;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ArticleService : IArticleService
{
    private readonly NoonpostDbContext _context;
    private readonly IBaseService _baseService;

    public ArticleService(NoonpostDbContext context, IBaseService baseService)
    {
        _context = context;
        _baseService = baseService;
    }

    public async Task<int> ArticleCommentsCount(long articleCId)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.CId == articleCId);

        return await _context.Comments
        .CountAsync(c => Equals(c.ArticleId, article.Id) && Equals(c.ParentId, null));
    }

    public async Task<int> ArticleAcceptedCommentsCount(long articleCId)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.CId == articleCId);

        return await _context.Comments.CountAsync(c => Equals(c.ArticleId, article.Id) &&
            Equals(c.ParentId, null) && c.IsAccepted);
    }

    public async Task<bool> IsExistsArticle(Guid articleId) => await _context.Articles.AnyAsync(a => Equals(a.Id, articleId));

    public async Task<Article> GetArticleByIdAsync(Guid articleId) => await _context.Articles.FindAsync(articleId);

    public async Task<Tuple<List<Article>, int>> GetArticlesByCategoryIdAsync(long categoryCId, int take, int skip)
    {
        var cat = await _context.Categories.FirstOrDefaultAsync(a => a.CId == categoryCId);

        var articles = _context.Articles
            .Include(a => a.User)
            .Include(a => a.Category)
            .Where(a => a.CategoryId == cat.Id)
            .OrderByDescending(a => a.CreateDate);

        return Tuple.Create(await articles.Skip(skip).Take(take).ToListAsync(), await articles.CountAsync(a => a.IsAccepted));
    }

    public async Task<Article> GetArticleForShowAsync(long articleCId, int take, int skip)
    {
        var model = await _context.Articles
            .Include(a => a.User)
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.CId == articleCId);

        model.View += 1;
        _context.Articles.Update(model);
        await _context.SaveChangesAsync();

        var comments = await _context.Comments
            .AsNoTrackingWithIdentityResolution()
            .Include(c => c.User)
            .Include(c => c.Replies.OrderByDescending(r => r.CreateDate))
            .ThenInclude(r => r.User)
            .Where(c => Equals(c.ArticleId, model.Id))
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
            CId = DateTime.Now.ToTimeStamp(),
            AuthorId = articleInfo.AuthorId,
            CategoryId = articleInfo.CategoryId,
            ImagesGuid = articleInfo.ArticleImageGuid,
            Title = articleInfo.Title,
            Text = articleInfo.Text,
            Tags = articleInfo.Tags,
            CreateDate = DateTime.Now
        };

        try
        {
            article.ImageName = await _baseService.SaveImageFile(image, "articles");
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
                CategoryId = a.CategoryId,
                AuthorId = a.AuthorId,
                ImageName = a.ImageName,
                Title = a.Title,
                Text = a.Text,
                Tags = a.Tags,
                Categories = GetAllCategoriesAsync().Result
            })
            .FirstOrDefaultAsync(a => a.ArticleId == articleId);
    }

    public async Task<bool> UpdateArticleAsync(EditArticleViewModel edit, IFormFile? newArticleImg)
    {
        try
        {
            var article = await _context.Articles.FindAsync(edit.ArticleId);
            article.CategoryId = edit.CategoryId;
            article.Title = edit.Title;
            article.Text = edit.Text;
            article.Tags = edit.Tags;
            article.UpdateDate = DateTime.Now;
            if (newArticleImg != null)
            {
                await _baseService.DeleteImageFile(article.ImageName, "articles");
                article.ImageName = await _baseService.SaveImageFile(newArticleImg, "articles");
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
            await _baseService.DeleteImageFile(article.ImageName, "articles");
            if (article.ImagesGuid != null)
            {
                var articleImages = await _context.ArticleImages
                    .Where(ai => Equals(ai.ArticleImageGuid, article.ImagesGuid))
                    .ToListAsync();

                foreach (var image in articleImages)
                {
                    await _baseService.DeleteImageFile(image.ImageName, "articles");
                    _context.ArticleImages.Remove(image);
                }
            }
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
        => await _context.Articles.CountAsync(a => a.IsAccepted);

    public async Task<List<Article>> GetArticlesForSlider()
        => await _context.Articles
            .Where(a => a.IsAccepted)
            .OrderByDescending(a => a.View)
            .Take(5)
            .ToListAsync();

    public async Task<List<Article>> GetArticlesForIndex(int take, int skip)
        => await _context.Articles
            .Include(a => a.User)
            .Include(a => a.Category)
            .Where(a => a.IsAccepted)
            .OrderByDescending(a => a.CreateDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

    public async Task<Tuple<List<Article>, int>> GetArticlesByFilter(string filter, int take, int skip)
    {
        var articles = _context.Articles
            .Include(a => a.User)
            .Include(a => a.Category)
            .Where(a => (a.Title.Contains(filter) || a.Tags.Contains(filter)) && a.IsAccepted);

        return Tuple.Create(await articles.Skip(skip).Take(take).ToListAsync(), await articles.CountAsync(a => a.IsAccepted));
    }

    public async Task<Tuple<string, string>> SaveUploadedArticleImage(IFormFile image)
    {
        try
        {
            var imageName = await _baseService.SaveImageFile(image, "articles");
            string imgSrc = $"/images/articles/{imageName}";
            return Tuple.Create(imgSrc, imageName);
        }
        catch { return Tuple.Create("", ""); }
    }

    public async Task<bool> AddArticleImage(Guid articleImageGuid, string imageName)
    {
        try
        {
            var articleImage = new ArticleImage
            {
                ArticleImageGuid = articleImageGuid,
                ImageName = imageName
            };
            await _context.ArticleImages.AddAsync(articleImage);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<List<Category>> GetCategoriesForNavBarAsync()
    {
        return await _context.Categories
            .Include(c => c.Articles)
            .OrderByDescending(c => c.Articles.Count())
            .Take(10)
            .ToListAsync();
    }

    public async Task<Tuple<long, string>> GetArticleCIdByArticleId(Guid articleId)
    {
        var article = await _context.Articles.FindAsync(articleId);
        return Tuple.Create(article.CId, article.Title);
    }
}

