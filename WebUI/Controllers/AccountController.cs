﻿using Application.Context;
using Domain.Entites.User;
using Infrastructure.Security;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebUI.Controllers;

public class AccountController : Controller
{
    private readonly NoonpostDbContext _context;

    public AccountController(NoonpostDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View();
        var isMobileExist = _context.Users.Any(u => u.Mobile == model.Mobile);
        if (isMobileExist)
        {
            ModelState.AddModelError(string.Empty, "شماره موبایل وارد شده قبلا استفاده شده است");
            return View();
        }

        var user = new User()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Mobile = model.Mobile,
            Password = PasswordEncoder.EncodePasswordMd5(model.Password),
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        return RedirectToAction(actionName: "Login", controllerName: "Account",
            new { IsRegistered = true });
    }

    [HttpGet]
    public IActionResult Login(bool IsRegistered = false)
    {
        ViewData["LoginAfterRegister"] = IsRegistered;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = _context.Users.FirstOrDefault(u => u.Mobile == model.Mobile);
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
            new Claim(ClaimTypes.Name, user.UserId.ToString())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var properties = new AuthenticationProperties()
        {
            IsPersistent = model.RememberMe
        };
        HttpContext.SignInAsync(principal, properties);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }
}
