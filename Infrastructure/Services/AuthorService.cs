using Application.Context;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AuthorService : IAuthorService
{
    private readonly NoonpostDbContext _context;

    public AuthorService(NoonpostDbContext context)
    {
        _context = context;
    }

    private async Task<Guid> GetAuthorIdByCId(long cId)
    {
        var author = await _context.Users.FirstOrDefaultAsync(u => u.CId == cId);
        return author.Id;
    }

    public async Task<int> AuthorArticlesCount(long authorCId)
    {
        var authorId = await GetAuthorIdByCId(authorCId);
        return await _context.Articles.CountAsync(a => Equals(a.AuthorId, authorId) && a.IsAccepted);
    }

    public async Task<AuthorInfoViewModel> GetAuthorInfoAsync(long authorCId, int take, int skip)
    {
        Guid authorId = await GetAuthorIdByCId(authorCId);

        var articles = await _context.Articles
            .Include(a => a.User)
            .Include(a => a.Category)
            .Where(a => a.AuthorId == authorId && a.IsAccepted)
            .OrderByDescending(a => a.CreateDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return await _context.Users
            .Include(u => u.Articles)
            .Where(u => Equals(u.Id, authorId))
            .Select(u => new AuthorInfoViewModel()
            {
                CId = u.CId,
                FullName = u.FullName,
                ImageName = u.ImageName,
                Email = u.Email,
                Description = u.Description,
                Articles = articles
            })
            .FirstOrDefaultAsync();
    }
}
