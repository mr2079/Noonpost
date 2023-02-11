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

    public async Task<int> AuthorArticlesCount(Guid authorId)
        => await _context.Articles.CountAsync(a => Equals(a.AuthorId, authorId));

    public async Task<AuthorInfoViewModel> GetAuthorInfoAsync(Guid authorId, int take, int skip)
        => await _context.Users
            .Include(u => u.Articles)
            .ThenInclude(a => a.User)
            .Where(u => Equals(u.Id, authorId))
            .Select(u => new AuthorInfoViewModel()
            {
                AuthorId = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ImageName = u.ImageName,
                Email = u.Email,
                Description = u.Description,
                Articles = u.Articles
                    .Where(a => a.IsAccepted)
                    .OrderByDescending(a => a.CreateDate)
                    .Skip(skip)
                    .Take(take)
                    .ToList()
            })
            .FirstOrDefaultAsync();
}
