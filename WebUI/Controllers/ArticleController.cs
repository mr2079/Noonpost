using Application.Context;
using Domain.Entites.Article;
using Domain.Entites.Comment;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;

namespace WebUI.Controllers;

[Authorize(Roles = "Admin,Author")]
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

    [AllowAnonymous]
    [HttpGet("/Article/{articleCId}/{articleTitle}")]
    public async Task<IActionResult> Show(long articleCId, Guid articleTitle, int commentPage = 1)
    {
        int takeComments = 5;
        int skipComments = takeComments * (commentPage - 1);
        var model = await _articleService.GetArticleForShowAsync(articleCId, takeComments, skipComments);
        int commentsCount = await _articleService.ArticleCommentsCount(articleCId);
        ViewData["CommentsCount"] = commentsCount;
        ViewData["AcceptedCommentsCount"] = await _articleService.ArticleAcceptedCommentsCount(articleCId);
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

        return RedirectToAction("Index", "User", 
            new { userCId = User.FindFirstValue("CId"), userName = User.FindFirstValue("UserName") });
    }

    [HttpGet("/Article/Edit/{articleId}")]
    public async Task<IActionResult> Edit(Guid articleId)
    {
        if (articleId == Guid.Empty) return NotFound();
        var model = await _articleService.EditArticleAsync(articleId);
        if (model == null) return NotFound();

        if (User.Identity.IsAuthenticated && model.AuthorId == Guid.Parse(User.Identity.Name))
            return View(model);

        return Unauthorized();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditArticleViewModel edit, IFormFile? newArticleImg)
    {
        if (!ModelState.IsValid) return View(edit);
        if (!await _articleService.IsExistsArticle(edit.ArticleId)) return NotFound();
        await _articleService.UpdateArticleAsync(edit, newArticleImg);

        return RedirectToAction("Index", "User",
            new { userCId = User.FindFirstValue("CId"), userName = User.FindFirstValue("UserName") });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid articleId, Guid authorId)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index", "User",
            new { userCId = User.FindFirstValue("CId"), userName = User.FindFirstValue("UserName") });

        var article = await _articleService.GetArticleByIdAsync(articleId);
        await _articleService.DeleteArticleAsync(article);

        return RedirectToAction("Index", "User",
            new { userCId = User.FindFirstValue("CId"), userName = User.FindFirstValue("UserName") });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateComment(CreateCommentViewModel userComment)
    {
        await _commentService.CreateComment(userComment);

        var result = await _articleService.GetArticleCIdByArticleId(userComment.ArticleId);

        return RedirectToAction("Show", "Article",
            new { articleCId = result.Item1, articleTitle = result.Item2 });
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommentReply(Guid articleId, Guid parentId, string text)
    {
        await _commentService.CreateComment(articleId, parentId, Guid.Parse(User.Identity.Name), text);

        var result = await _articleService.GetArticleCIdByArticleId(articleId);

        return RedirectToAction("Show", "Article",
            new { articleCId = result.Item1, articleTitle = result.Item2 });
    }

    [HttpPost]
    public async Task<IActionResult> EditComment(Guid commentId, string text)
    {
        var articleId = await _commentService.EditComment(commentId, text);

        var result = await _articleService.GetArticleCIdByArticleId(articleId);

        return RedirectToAction("Show", "Article",
            new { articleCId = result.Item1, articleTitle = result.Item2 });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteComment(Guid commentId, Guid articleId)
    {
        await _commentService.DeleteComment(commentId);

        var result = await _articleService.GetArticleCIdByArticleId(articleId);

        return RedirectToAction("Show", "Article",
            new { articleCId = result.Item1, articleTitle = result.Item2 });
    }

    [HttpPost]
    public async Task<ActionResult> UploadArticleImage(IFormFile upload, Guid articleImageGuid)
    {
        var result = await _articleService.SaveUploadedArticleImage(upload);
        await _articleService.AddArticleImage(articleImageGuid, result.Item2);

        return Json(new { url = result.Item1 });
    }

    [AllowAnonymous]
    [HttpGet("/Category/{categoryCId}/{categoryTitle}")]
    public async Task<IActionResult> Category(long categoryCId, string categoryTitle, int page = 1)
    {
        int take = 20;
        int skip = take * (page - 1);

        var result = await _articleService.GetArticlesByCategoryIdAsync(categoryCId, take, skip);
        var pagesCount = (result.Item2 + take - 1) / take;

        return View(Tuple.Create(result.Item1, pagesCount, categoryCId, categoryTitle.Replace("-", " ")));
    }
}
