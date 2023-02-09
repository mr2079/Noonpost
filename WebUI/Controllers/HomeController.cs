using Application.Context;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IArticleService _articleService;

    public HomeController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet("/404")]
    public IActionResult NotFoundPage() => View();

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        var slider = await _articleService.GetArticlesForSlider();

        var take = 6;
        var skip = take * (page - 1);

        var articles = await _articleService.GetArticlesForIndex(take, skip);

        var pagesCount = ((await _articleService.ArticlesCountAsync()) + take - 1) / take;

        return View(Tuple.Create(slider, articles, pagesCount));
    }

    [HttpGet("/Search")]
    public async Task<IActionResult> Search(string filter, int page = 1)
    {
        if (filter == null || filter == string.Empty) return NotFound();

        int take = 6;
        int skip = take * (page - 1);

        var articles = await _articleService.GetArticlesByFilter(filter, take, skip);

        var pageCount = (articles.Count + take - 1) / take;

        return View(Tuple.Create(articles, pageCount));
    }
}
