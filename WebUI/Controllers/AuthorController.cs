using Application.Context;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebUI.Controllers;

public class AuthorController : Controller
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet("/Author/{authorCId}/{authorName}")]
    public async Task<IActionResult> Index(long authorCId, string authorName, int page = 1)
    {
        if (authorCId == 0) return NotFound();

        if (User.Identity.IsAuthenticated && long.Parse(User.FindFirstValue("CId")) == authorCId)
            return RedirectToAction("Index", "User",
                new { userCId = authorCId, userName = authorName });

        int take = 6;
        int skip = take * (page - 1);

        var author = await _authorService.GetAuthorInfoAsync(authorCId, take, skip);

        if (author == null) return NotFound();

        if (author.Articles.Count() > 0)
        {
            int articlesCount = await _authorService.AuthorArticlesCount(author.CId);
            ViewData["ArticlesCount"] = articlesCount;
            ViewData["PageCount"] = (articlesCount + take - 1) / take;
        }

        return View(author);
    }
}
