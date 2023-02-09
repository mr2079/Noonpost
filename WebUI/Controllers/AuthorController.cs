using Application.Context;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers;

public class AuthorController : Controller
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet("/Author/{authorId}")]
    public async Task<IActionResult> Index(Guid authorId, int page = 1)
    {
        if (authorId == Guid.Empty) return NotFound();

        int take = 6;
        int skip = take * (page - 1);

        var author = await _authorService.GetAuthorInfoAsync(authorId, take, skip);

        if (author == null) return NotFound();

        if (author.Articles.Count() > 0)
        {
            int articlesCount = await _authorService.AuthorArticlesCount(author.AuthorId);
            ViewData["ArticlesCount"] = articlesCount;
            ViewData["PageCount"] = (articlesCount + take - 1) / take;
        }

        return View(author);
    }
}
