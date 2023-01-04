using Application.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace WebUI.Controllers;

public class HomeController : Controller
{
    private readonly NoonpostDbContext _context;

    public HomeController(NoonpostDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        var slider = await _context.Articles
            .OrderByDescending(a => a.View)
            .Take(5)
            .ToListAsync();

        var take = 6;
        var skip = take * (page - 1);

        var articles = await _context.Articles
            .Include(a => a.User)
            .OrderByDescending(a => a.CreateDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        var pagesCount = ((await _context.Articles.CountAsync()) + take - 1) / take;

        return View(Tuple.Create(slider, articles, pagesCount));
    }

    [HttpGet("/404")]
    public IActionResult NotFoundPage() => View();

    [HttpGet("/Search")]
    public async Task<IActionResult> Search(string filter, int page = 1)
    {
        if (filter == null || filter == string.Empty) return NotFound();

        int take = 6;
        int skip = take * (page - 1);
        
        var articles = await _context.Articles
            .Include(a => a.User)
            .Where(a => a.Title.Contains(filter) || a.Tags.Contains(filter))
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        var pageCount = (articles.Count + take - 1) / take;

        return View(Tuple.Create(articles, pageCount));
    }
}
