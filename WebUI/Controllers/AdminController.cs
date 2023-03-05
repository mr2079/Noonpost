using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("/Admin/Dashboard")]
    public async Task<IActionResult> Index()
    {
        return View(await _adminService.GetAdminDashboardInfo());
    }

    [HttpGet("/Admin/Users")]
    public async Task<IActionResult> ManageUsers(int page = 1)
    {
        int take = 20;
        int skip = take * (page - 1);
        var users = await _adminService.GetAllUsers(take, skip);
        int usersCount = await _adminService.AllUsersCount();
        int pagesCount = (usersCount + take - 1) / take;

        return View(Tuple.Create(users, pagesCount, page, take));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _adminService.DeleteUser(userId);
        return RedirectToAction("ManageUsers", "Admin");
    }

    [HttpPost]
    public async Task EditUserRole(Guid userId, string role)
    {
        await _adminService.EditUserRole(userId, role);
        await Task.CompletedTask;
    }

    [HttpGet("/Admin/Articles")]
    public async Task<IActionResult> ManageArticles(int page = 1)
    {
        int take = 20;
        int skip = take * (page - 1);
        var articles = await _adminService.GetAllArticles(take, skip);
        int articlesCount = await _adminService.AllArticlesCount();
        int pagesCount = (articlesCount + take - 1) / take;

        return View(Tuple.Create(articles, pagesCount, page, take));
    }

    public async Task<IActionResult> AcceptArticle(Guid articleId)
    {
        await _adminService.AcceptArticle(articleId);
        return RedirectToAction("Show", "Article",
            new { articleId });
    }

    public async Task<IActionResult> DeclineArticle(Guid articleId)
    {
        await _adminService.DeclineArticle(articleId);
        return RedirectToAction("Show", "Article",
            new { articleId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteArticle(Guid articleId)
    {
        await _adminService.DeleteArticle(articleId);
        return RedirectToAction("ManageArticles", "Admin");
    }

    [HttpPost]
    public async Task<JsonResult> AcceptComment(Guid commentId)
    {
        if (await _adminService.AcceptComment(commentId))
            return new JsonResult(true);

        return new JsonResult(false);
    }

    [HttpPost]
    public async Task<JsonResult> DeleteComment(Guid commentId)
    {
        if (await _adminService.DeleteComment(commentId))
            return new JsonResult(true);

        return new JsonResult(false);
    }

    [HttpGet("/Admin/Comments")]
    public async Task<IActionResult> ManageComments(int page = 1)
    {
        int take = 20;
        int skip = take * (page - 1);
        var result = await _adminService.GetArticlesWithNewComments();
        var articlesCount = result.Item2;
        var pagesCount = (articlesCount + take - 1) / take;

        return View(Tuple.Create(result.Item1, pagesCount, page, take));
    }

    [HttpGet("/Admin/Categories")]
    public async Task<IActionResult> ManageCategories(int page = 1)
    {
        int take = 20;
        int skip = take * (page - 1);
        var model = await _adminService.GetAllCategoriesAsync();
        int categoriesCount = await _adminService.AllCategoriesCount();
        int pagesCount = (categoriesCount + take - 1) / take;

        return View(Tuple.Create(model, pagesCount, page, take));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(string categoryTitle)
    {
        await _adminService.CreateCategoryAsync(categoryTitle);

        return RedirectToAction("ManageCategories", "Admin");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCategory(Guid categoryId, string categoryTitle)
    {
        await _adminService.UpdateCategoryAsync(categoryId, categoryTitle);

        return RedirectToAction("ManageCategories", "Admin");
    }
}
