using Application.Context;
using Domain.Entites.User;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers;

public class AuthorController : Controller
{
    private readonly NoonpostDbContext _context;

    public AuthorController(NoonpostDbContext context)
    {
        _context = context;
    }

    [HttpGet("/Author/{authorId}")]
    public async Task<IActionResult> Index(Guid authorId, int page = 1)
    {
        if (authorId == null || authorId == Guid.Empty) return NotFound();

        int take = 6;
        int skip = take * (page - 1);

        var author = await _context.Users
            .Include(u => u.Articles)
            .ThenInclude(a => a.User)
            .Where(u => u.Role == "author" && u.UserId == authorId)
            .Select(u => new AuthorInfoViewModel()
            {
                AuthorId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ImageName = u.ImageName,
                Email = u.Email,
                Description = u.Description,
                Articles = u.Articles
                    .OrderByDescending(a => a.CreateDate)
                    .Skip(skip)
                    .Take(take)
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (author == null) return NotFound();

        if (author.Articles.Count() > 0)
        {
            var articlesCount = _context.Articles
                .Where(a => a.AuthorId == authorId)
                .Count();

            ViewData["ArticlesCount"] = articlesCount;
            ViewData["PageCount"] = articlesCount / take;
        }

        return View(author);
    }
}
