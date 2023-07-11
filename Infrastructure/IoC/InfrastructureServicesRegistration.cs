using Infrastructure.Services.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IoC;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddTransient<IBaseService, BaseService>();

        return services;
    }
}
