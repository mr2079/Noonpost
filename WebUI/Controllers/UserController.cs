using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Authorize]
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

    [HttpGet("/User/{userCId}/{userName}")]
    public async Task<IActionResult> Index(long userCId, string userName, int page = 1)
    {
        if (userCId == 0) return Redirect("/");

        int take = 6;
        int skip = take * (page - 1);

        var model = await _userService.GetUserPanelInfo(userCId, take, skip);

        if (model == null) return NotFound();

        if (model.Articles.Count() > 0)
        {
            var articlesCount = await _authorService.AuthorArticlesCount(userCId);
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

        return RedirectToAction("Index", "User", new { userCId = info.CId, userName = info.FullName });
    }
}
