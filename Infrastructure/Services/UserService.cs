using Application.Context;
using Domain.Entites.User;
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
    {
        var articles = await _context.Articles
            .Include(a => a.Category)
            .Where(a => a.AuthorId == userId)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return await _context.Users
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
                Articles = articles
            })
            .FirstOrDefaultAsync(u => Equals(u.UserId, userId));
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

    public async Task<Tuple<string, string>> GetUserInfoForNavigationBar(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return Tuple.Create(string.Empty, string.Empty);
        return Tuple.Create($"{user.FirstName} {user.LastName}", user.ImageName);
    }
}
