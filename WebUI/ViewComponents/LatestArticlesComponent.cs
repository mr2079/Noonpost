using Application.Context;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.ViewComponents;

public class LatestArticlesComponent : ViewComponent
{
    private readonly NoonpostDbContext _context;

    public LatestArticlesComponent(NoonpostDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var articles = await _context.Articles
            .OrderByDescending(a => a.CreateDate)
            .Take(4)
            .Select(a => new LatestArticleViewModel() 
            {
                CId = a.CId,
                Title= a.Title,
                ImageName = a.ImageName,
                CreateDate = a.CreateDate
            })
            .ToListAsync();
        return await Task.FromResult((IViewComponentResult)View("LatestArticles", articles));
    }
}

