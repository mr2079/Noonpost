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

    public IActionResult Index()
    {
        return View();
    }

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
            new { articleId = articleId });
    }

    public async Task<IActionResult> DeclineArticle(Guid articleId)
    {
        await _adminService.DeclineArticle(articleId);
        return RedirectToAction("Show", "Article",
            new { articleId = articleId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteArticle(Guid articleId)
    {
        await _adminService.DeleteArticle(articleId);
        return RedirectToAction("ManageArticles", "Admin");
    }
}
