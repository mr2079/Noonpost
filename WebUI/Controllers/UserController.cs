using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class UserController : Controller
{
    private readonly IAuthorService _authorService;
    private readonly IUserService _userService; 

    public UserController(IAuthorService authorService,
        IUserService userService)
    {
        _authorService = authorService;
        _userService = userService;
    }

    [HttpGet("/User/{userId}")]
    public async Task<IActionResult> Index(string userId, int page = 1)
    {
        if (userId == null || userId == string.Empty) return Redirect("/");

        int take = 6;
        int skip = take * (page - 1);

        var model = await _userService.GetUserPanelInfo(Guid.Parse(userId), take, skip);

        if (model == null) return NotFound();

        if (model.Articles.Count() > 0)
        {
            var articlesCount = await _authorService.AuthorArticlesCount(Guid.Parse(userId));
            ViewData["ArticlesCount"] = articlesCount;
            ViewData["PageCount"] = (articlesCount + take - 1) / take;
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUserInfo(UserPanelInfoViewModel info)
    {
        if (!ModelState.IsValid) return View(info);
        await _userService.UpdateUser(info);

        return RedirectToAction("Index", "User", new { userId = info.UserId });
    }
}
