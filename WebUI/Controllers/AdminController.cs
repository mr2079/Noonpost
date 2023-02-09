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

        return View(Tuple.Create(users, pagesCount));
    }

    public async Task<IActionResult> ManageArticles(int page = 1)
    {
        int take = 20;
        int skip = take * (page - 1);
        var articles = await _adminService.GetAllArticles(take, skip);
        int articlesCount = await _adminService.AllArticlesCount();
        int pagesCount = (articlesCount + take - 1) / take;

        return View(Tuple.Create(articles, pagesCount));
    }
}
