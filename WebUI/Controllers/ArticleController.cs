using Application.Context;
using Domain.Entites.Article;
using Domain.Entites.Comment;
using Infrastructure.Generator;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers;

public class ArticleController : Controller
{
    private readonly NoonpostDbContext _context;
    private readonly IArticleService _articleService;

    public ArticleController(NoonpostDbContext context, IArticleService articleService)
    {
        _context = context;
        _articleService = articleService;
    }

    [HttpGet("/Article/{articleId}")]
    public async Task<IActionResult> Show(Guid articleId, int commentPage = 1)
    {
        if (articleId == Guid.Empty) return NotFound();
        int takeComments = 5;
        int skipComments = takeComments * (commentPage - 1);
        var model = await _articleService.GetArticleForShowAsync(articleId, takeComments, skipComments);
        int commentsCount = model.Comments.Where(c => c.ParentId == null).Count();
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
    public IActionResult CreateComment(CreateCommentViewModel userComment)
    {
        var comment = new Comment()
        {
            ArticleId = userComment.ArticleId,
            UserId = userComment.UserId,
            Text = userComment.CommentText,
            IsAccepted = true,
            CreateDate = DateTime.Now,
        };
        _context.Comments.Add(comment);
        _context.SaveChanges();

        return RedirectToAction("Show", "Article", new { articleId = userComment.ArticleId });
    }

    [HttpPost]
    public IActionResult CreateCommentReply(Guid articleId, Guid parentId, string text)
    {
        var comment = new Comment()
        {
            UserId = Guid.Parse(User.Identity.Name),
            ArticleId = articleId,
            ParentId = parentId,
            Text = text
        };

        _context.Comments.Add(comment);
        _context.SaveChanges();

        return RedirectToAction("Show", "Article",
            new { articleId = articleId });
    }

    [HttpPost]
    public async Task<IActionResult> EditComment(Guid commentId, string text)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        comment.Text = text;
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Show", "Article",
            new { articleId = comment.ArticleId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Show", "Article",
            new { articleId = comment.ArticleId });
    }
}
