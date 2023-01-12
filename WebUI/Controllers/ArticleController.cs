using Application.Context;
using Domain.Entites.Article;
using Domain.Entites.Comment;
using Infrastructure.Generator;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace WebUI.Controllers;

public class ArticleController : Controller
{
    private readonly NoonpostDbContext _context;

    public ArticleController(NoonpostDbContext context)
    {
        _context = context;
    }

    [HttpGet("/Article/{articleId}")]
    public async Task<IActionResult> Show(Guid articleId, int commentPage = 1)
    {
        if (articleId == null || articleId == Guid.Empty) return NotFound();

        var model = await _context.Articles
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == articleId);

        model.View += 1;
        _context.Articles.Update(model);
        await _context.SaveChangesAsync();

        int takeComments = 5;
        int skipComments = takeComments * (commentPage - 1);

        var comments = await _context.Comments
            .AsNoTrackingWithIdentityResolution()
            .Include(c => c.User)
            .Include(c => c.Replies.OrderByDescending(r => r.CreateDate))
            .ThenInclude(r => r.User)
            .ToListAsync();

        model.Comments = comments
            .Where(c => c.ParentId == null)
            .OrderByDescending(c => c.CreateDate)
            .AsParallel()
            .Skip(skipComments)
            .Take(takeComments)
            .ToList();

        int commentsCount = comments.Where(c => c.ParentId == null).Count();

        ViewData["CommentsCount"] = commentsCount;
        ViewData["CommentsPageCount"] = (commentsCount + takeComments - 1) / takeComments;

        return View(model);
    }

    [HttpGet("/Article/Create")]
    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(CreateArticleViewModel articleInfo, IFormFile Image)
    {
        if (!ModelState.IsValid) return View(articleInfo);
        if (articleInfo.Text == null)
        {
            ModelState.AddModelError("Text", "فیلد متن مقاله الزامی می باشد");
            return View(articleInfo);
        }

        var article = new Article()
        {
            AuthorId = articleInfo.AuthorId,
            Title = articleInfo.Title,
            Text = articleInfo.Text,
            Tags = articleInfo.Tags,
            CreateDate = DateTime.Now,
            ImageName = NameGenerator.Generate() + Path.GetExtension(Image.FileName),
            IsAccepted = true
        };

        var imagePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\images\\articles", article.ImageName);
        using (var fs = new FileStream(imagePath, FileMode.Create)) Image.CopyTo(fs);

        _context.Articles.Add(article);
        _context.SaveChanges();

        return RedirectToAction("Index", "User", new { userId = articleInfo.AuthorId });
    }

    [HttpGet("/Article/Edit/{articleId}")]
    public IActionResult Edit(Guid articleId)
    {
        if (articleId == null || articleId == Guid.Empty) return NotFound();
        var model = _context.Articles
            .Select(a => new EditArticleViewModel()
            {
                ArticleId = a.Id,
                AuthorId = a.AuthorId,
                ImageName = a.ImageName,
                Title = a.Title,
                Text = a.Text,
                Tags = a.Tags,
            })
            .FirstOrDefault(a => a.ArticleId == articleId);
        if (model == null) return NotFound();

        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(EditArticleViewModel edit, IFormFile? newArticleImg)
    {
        if (!ModelState.IsValid) return View(edit);
        var article = _context.Articles.Find(edit.ArticleId);
        if (article == null) return NotFound();
        article.Title = edit.Title;
        article.Tags = edit.Tags;
        article.UpdateDate = DateTime.Now;
        if (newArticleImg != null)
        {
            var oldImagePath = Path
                .Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\articles", article.ImageName);
            if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);

            article.ImageName = NameGenerator.Generate() + Path.GetExtension(newArticleImg.FileName);
            var newImagePath = Path
                .Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\articles", article.ImageName);
            using (var fs = new FileStream(newImagePath, FileMode.Create)) newArticleImg.CopyTo(fs);
        }
        _context.Articles.Update(article);
        _context.SaveChanges();

        return RedirectToAction("Index", "User", new { userId = edit.AuthorId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Guid articleId, Guid authorId)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index", "User",
            new { userId = authorId, isArticleDeleteSucceeded = false.ToString() });

        var article = _context.Articles.Find(articleId);
        if (article == null) return RedirectToAction("Index", "User",
            new { userId = authorId, isArticleDeleteSucceeded = false.ToString() });

        try
        {
            _context.Articles.Remove(article);
            _context.SaveChanges();
            return RedirectToAction("Index", "User",
            new { userId = authorId, isArticleDeleteSucceeded = true.ToString() });
        }
        catch
        {
            return RedirectToAction("Index", "User",
            new { userId = authorId, isArticleDeleteSucceeded = false.ToString() });
        }
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
