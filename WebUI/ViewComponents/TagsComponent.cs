using Application.Context;
using Domain.Entites.Article;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.ViewComponents;

public class TagsComponent : ViewComponent
{
    private readonly NoonpostDbContext _context;

    public TagsComponent(NoonpostDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var tags = await _context.Articles
            .OrderByDescending(a => a.View)
            .Where(a => a.Tags != null)
            .Select(a => a.Tags)
            .Take(5)
            .ToListAsync();

        return await Task.FromResult((IViewComponentResult)View("Tags", tags));
    }
}

