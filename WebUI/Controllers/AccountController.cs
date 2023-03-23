using Infrastructure.Security;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebUI.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Register(string returnUrl = "/")
    {
        if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = "/")
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid) return View();
        if (await _userService.IsMobileExists(model.Mobile))
        {
            ModelState.AddModelError(string.Empty, "شماره موبایل وارد شده قبلا استفاده شده است");
            return View();
        }

        await _userService.AddUser(model);

        return RedirectToAction("Login", "Account",
            new { isRegistered = true, returnUrl = returnUrl });
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = "/", bool isRegistered = false)
    {
        if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

        ViewData["LoginAfterRegister"] = isRegistered;
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid) return View(model);
        var user = await _userService.GetUserByMobile(model.Mobile);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "حساب کاربری با شماره موبایل وارد شده، وجود ندارد");
            return View(model);
        }
        if (user.Password != PasswordEncoder.EncodePasswordMd5(model.Password))
        {
            ModelState.AddModelError(string.Empty, "کلمه عبور وارد شده اشتباه است");
            return View(model);
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim("CId", user.CId.ToString()),
            new Claim("UserName", user.FullName)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var properties = new AuthenticationProperties()
        {
            IsPersistent = model.RememberMe
        };
        await HttpContext.SignInAsync(principal, properties);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && !string.Equals(returnUrl, "/"))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "User",
            new
            {
                userCId = user.CId,
                userName = user.FullName.Replace(" ", "-")
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }
}
