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

    public async Task<List<Article>> GetAllArticles(int take, int skip)
        => await _context.Articles
            .Include(a => a.User)
            .OrderByDescending(a => a.CreateDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
}
