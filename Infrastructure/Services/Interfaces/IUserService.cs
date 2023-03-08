using Domain.Entites.User;
using Infrastructure.ViewModels;

namespace Infrastructure.Services.Interfaces;

public interface IUserService
{
    Task<User> GetUserById(Guid userId);
    Task<User> GetUserByMobile(string mobile);
    Task<bool> IsMobileExists(string mobile);
    Task<bool> AddUser(RegisterViewModel model);
    Task<bool> UpdateUser(UserPanelInfoViewModel info);
    Task<UserPanelInfoViewModel> GetUserPanelInfo(long userCId, int take, int skip);
    Task<UserNavBarInfoViewModel> GetUserInfoForNavigationBar(Guid userId);
}
