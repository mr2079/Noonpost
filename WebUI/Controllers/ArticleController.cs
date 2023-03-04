using Application.Context;
using Domain.Entites.Comment;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace WebUI.Controllers;

[Authorize(Roles = "Admin,Author")]
public class ArticleController : Controller
{
    private readonly IArticleService _articleService;
    private readonly ICommentService _commentService;

    public ArticleController(IArticleService articleService,
        ICommentService commentService,
        IAdminService adminService)
    {
        _articleService = articleService;
        _commentService = commentService;
    }

    [AllowAnonymous]
    [HttpGet("/Article/{articleId}")]
    public async Task<IActionResult> Show(Guid articleId, Guid? articleTitle, int commentPage = 1)
    {
        if (articleId == Guid.Empty) return NotFound();
        int takeComments = 5;
        int skipComments = takeComments * (commentPage - 1);
        var model = await _articleService.GetArticleForShowAsync(articleId, takeComments, skipComments);
        int commentsCount = await _articleService.ArticleCommentsCount(articleId);
        ViewData["CommentsCount"] = commentsCount;
        ViewData["AcceptedCommentsCount"] = await _articleService.ArticleAcceptedCommentsCount(articleId);
        ViewData["CommentsPageCount"] = (commentsCount + takeComments - 1) / takeComments;

        return View(model);
    }

    [HttpGet("/Article/Create")]
    public async Task<IActionResult> Create()
    {
        var model = new CreateArticleViewModel
        {
            ArticleImageGuid = Guid.NewGuid(),
            Categories = await _articleService.GetAllCategoriesAsync(),
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateArticleViewModel articleInfo, IFormFile Image)
    {
        if (!ModelState.IsValid) return View(articleInfo);
        if (articleInfo.Text == null || articleInfo.CategoryId == Guid.Empty)
        {
            if (articleInfo.CategoryId == Guid.Empty)
                ModelState.AddModelError("CategoryId", "فیلد عنوان دسته بندی الزامی می باشد");
            if (articleInfo.Text == null)
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
        if (!await _articleService.IsExistsArticle(edit.ArticleId)) return NotFound();
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
            new { articleId });
    }

    [HttpPost]
    public async Task<IActionResult> EditComment(Guid commentId, string text)
    {
        Guid articleId = await _commentService.EditComment(commentId, text);

        return RedirectToAction("Show", "Article",
            new { articleId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteComment(Guid commentId, Guid articleId)
    {
        await _commentService.DeleteComment(commentId);

        return RedirectToAction("Show", "Article",
            new { articleId });
    }

    [HttpPost]
    public async Task<ActionResult> UploadArticleImage(IFormFile upload, Guid articleImageGuid)
    {
        var result = await _articleService.SaveUploadedArticleImage(upload);
        await _articleService.AddArticleImage(articleImageGuid, result.Item2);

        return Json(new { url = result.Item1 });
    }

    [AllowAnonymous]
    [HttpGet("/Category/{categoryTitle}/{categoryId}")]
    public async Task<IActionResult> Category(Guid categoryId, string categoryTitle, int page = 1)
    {
        int take = 20;
        int skip = take * (page - 1);

        var result = await _articleService.GetArticlesByCategoryIdAsync(categoryId, take, skip);
        var pagesCount = (result.Item2 + take - 1) / take;

        return View(Tuple.Create(result.Item1, pagesCount, categoryId, categoryTitle));
    }
}
