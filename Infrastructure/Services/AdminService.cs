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

    public AdminService(NoonpostDbContext context)
    {
        _context = context;
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
}
