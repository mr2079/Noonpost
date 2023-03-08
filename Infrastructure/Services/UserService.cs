using Application.Context;
using Domain.Entites.User;
using Infrastructure.Converter;
using Infrastructure.Security;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly NoonpostDbContext _context;
    private readonly IBaseService _baseService;

    public UserService(NoonpostDbContext context, IBaseService baseService)
    {
        _context = context;
        _baseService = baseService;
    }

    public async Task<bool> AddUser(RegisterViewModel model)
    {
        var user = new User()
        {
            CId = DateTime.Now.ToTimeStamp(),
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

    public async Task<UserPanelInfoViewModel> GetUserPanelInfo(long userCId, int take, int skip)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.CId == userCId);

        var articles = await _context.Articles
            .Include(a => a.Category)
            .Where(a => a.AuthorId == user.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return await _context.Users
            .Include(u => u.Articles)
            .Where(u => u.Id == user.Id)
            .Select(u => new UserPanelInfoViewModel()
            {
                UserId = u.Id,
                CId = u.CId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Mobile = u.Mobile,
                Email = u.Email,
                ImageName = u.ImageName,
                Description = u.Description,
                Articles = articles
            })
            .FirstOrDefaultAsync();
    }

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
                    await _baseService.DeleteImageFile(user.ImageName, "users");
                }

                user.ImageName = await _baseService.SaveImageFile(info.Image, "users");
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<UserNavBarInfoViewModel> GetUserInfoForNavigationBar(Guid userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserNavBarInfoViewModel
            {
                CId = u.CId,
                FullName = u.FullName,
                ImageName = u.ImageName
            })
            .FirstOrDefaultAsync();
    }
}
