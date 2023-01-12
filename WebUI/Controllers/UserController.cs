using Application.Context;
using Infrastructure.Generator;
using Infrastructure.Security;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers;

public class UserController : Controller
{
    private readonly NoonpostDbContext _context;

    public UserController(NoonpostDbContext context)
    {
        _context = context;
    }

    [HttpGet("/User/{userId}")]
    public async Task<IActionResult> Index(string userId, int page = 1)
    {
        if (userId == null || userId == string.Empty) return Redirect("/");

        int take = 6;
        int skip = take * (page - 1);

        var model = await _context.Users
            .Include(u => u.Articles)
            .Select(u => new UserPanelInfoViewModel()
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Mobile = u.Mobile,
                Email = u.Email,
                ImageName = u.ImageName,
                Description = u.Description,
                Articles = u.Articles
                .OrderByDescending(a => a.CreateDate)
                .Skip(skip)
                .Take(take)
                .ToList()
            })
            .FirstOrDefaultAsync(u => u.UserId == Guid.Parse(userId));

        if (model == null) return NotFound();

        if (model.Articles.Count() > 0)
        {
            var articlesCount = _context.Articles
                .Where(a => a.AuthorId == Guid.Parse(userId))
                .Count();

            ViewData["ArticlesCount"] = articlesCount;
            ViewData["PageCount"] = (articlesCount + take - 1) / take;
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateUserInfo(UserPanelInfoViewModel info)
    {
        if (!ModelState.IsValid) return View(info);
        var user = _context.Users.Find(info.UserId);
        user.FirstName = info.FirstName;
        user.LastName = info.LastName;
        user.Mobile = info.Mobile;
        user.Email = info.Email;
        user.Description = info.Description;
        user.UpdateDate = DateTime.Now;
        if (info.Password != null)
        {
            user.Password = PasswordEncoder.EncodePasswordMd5(info.Password);
        }
        if (info.Image != null)
        {
            if (user.ImageName != "Default.jpg")
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\users", user.ImageName);
                if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
            }

            user.ImageName = NameGenerator.Generate() + Path.GetExtension(info.Image.FileName);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\users", user.ImageName);
            using (var fs = new FileStream(imagePath, FileMode.Create)) info.Image.CopyTo(fs);
        }
        _context.Users.Update(user);
        _context.SaveChanges();

        return RedirectToAction("Index", "User", new { userId = user.Id });
    }
}
