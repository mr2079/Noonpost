using Application.Context;
using Domain.Entites.User;
using Infrastructure.Security;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Infrastructure.Generator;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly NoonpostDbContext _context;

    public UserService(NoonpostDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddUser(RegisterViewModel model)
    {
        var user = new User()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Mobile = model.Mobile,
            Password = PasswordEncoder.EncodePasswordMd5(model.Password)
        };

        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<User> GetUserById(Guid userId)
        => await _context.Users.FindAsync(userId);

    public async Task<User> GetUserByMobile(string mobile)
        => await _context.Users.FirstOrDefaultAsync(u => string.Equals(u.Mobile, mobile));

    public async Task<UserPanelInfoViewModel> GetUserPanelInfo(Guid userId, int take, int skip)
        => await _context.Users
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
            .FirstOrDefaultAsync(u => Equals(u.UserId, userId));

    public async Task<bool> IsMobileExists(string mobile)
        => await _context.Users.AnyAsync(u => string.Equals(u.Mobile, mobile));

    public async Task<bool> UpdateUser(UserPanelInfoViewModel info)
    {
        try
        {
            var user = await GetUserById(info.UserId);
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
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }
}
