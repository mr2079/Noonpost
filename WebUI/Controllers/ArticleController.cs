using Application.Context;
using Domain.Entites.Comment;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class ArticleController : Controller
{
    private readonly IArticleService _articleService;
    private readonly ICommentService _commentService;

    public ArticleController(IArticleService articleService,
        ICommentService commentService)
    {
        _articleService = articleService;
        _commentService = commentService;
    }

    [HttpGet("/Article/{articleId}")]
    public async Task<IActionResult> Show(Guid articleId, int commentPage = 1)
    {
        if (articleId == Guid.Empty) return NotFound();
        int takeComments = 5;
        int skipComments = takeComments * (commentPage - 1);
        var model = await _articleService.GetArticleForShowAsync(articleId, takeComments, skipComments);
        int commentsCount = await _articleService.ArticleCommentsCount(articleId);
        ViewData["CommentsCount"] = commentsCount;
        ViewData["CommentsPageCount"] = (commentsCount + takeComments - 1) / takeComments;

        return View(model);
    }

    [HttpGet("/Article/Create")]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateArticleViewModel articleInfo, IFormFile Image)
    {
        if (!ModelState.IsValid) return View(articleInfo);
        if (articleInfo.Text == null)
        {
            ModelState.AddModelError("Text", "فیلد متن مقاله الزامی می باشد");
            return View(articleInfo);
        }
        await _articleService.CreateArticleAsync(articleInfo, Image);

        return RedirectToAction("Index", "User", new { userId = articleInfo.AuthorId });
    }

    [HttpGet("/Article/Edit/{articleId}")]
    public async Task<IActionResult> Edit(Guid articleId)
    {
        if (articleId == Guid.Empty) return NotFound();
        var model = await _articleService.EditArticleAsync(articleId);
        if (model == null) return NotFound();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditArticleViewModel edit, IFormFile? newArticleImg)
    {
        if (!ModelState.IsValid) return View(edit);
        if (await _articleService.IsExistsArticle(edit.ArticleId)) return NotFound();
        await _articleService.UpdateArticleAsync(edit, newArticleImg);

        return RedirectToAction("Index", "User", new { userId = edit.AuthorId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid articleId, Guid authorId)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index", "User",
            new { userId = authorId, isArticleDeleteSucceeded = false.ToString() });

        var article = await _articleService.GetArticleByIdAsync(articleId);
        if (article == null) return RedirectToAction("Index", "User",
            new { userId = authorId, isArticleDeleteSucceeded = false.ToString() });

        if (await _articleService.DeleteArticleAsync(article)) return RedirectToAction("Index", "User",
            new { userId = authorId, isArticleDeleteSucceeded = true.ToString() });

        return RedirectToAction("Index", "User",
            new { userId = authorId, isArticleDeleteSucceeded = false.ToString() });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateComment(CreateCommentViewModel userComment)
    {
        await _commentService.CreateComment(userComment);

        return RedirectToAction("Show", "Article", new { articleId = userComment.ArticleId });
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommentReply(Guid articleId, Guid parentId, string text)
    {
        await _commentService.CreateComment(articleId, parentId, Guid.Parse(User.Identity.Name), text);

        return RedirectToAction("Show", "Article",
            new { articleId = articleId });
    }

    [HttpPost]
    public async Task<IActionResult> EditComment(Guid commentId, string text)
    {
        Guid articleId = await _commentService.EditComment(commentId, text);

        return RedirectToAction("Show", "Article",
            new { articleId = articleId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        Guid articleId = await _commentService.DeleteComment(commentId);

        return RedirectToAction("Show", "Article",
            new { articleId = articleId });
    }
}
