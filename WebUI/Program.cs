using Application.Context;
using Infrastructure.IoC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.AccessDeniedPath = "/401";
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
});

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnectionString") ??
    throw new InvalidOperationException("Connection string 'DefaultConnectionString' not found.");

builder.Services.AddDbContext<NoonpostDbContext>(options =>
    options.UseSqlServer(defaultConnectionString));

builder.Services.AddInfrastructureServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NoonpostDbContext>();
    context.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseStatusCodePages(async context =>
    {
        switch (context.HttpContext.Response.StatusCode)
        {
            case 401:
                context.HttpContext.Response.Redirect("/401");
                break;
            default:
                context.HttpContext.Response.Redirect("/404");
                break;
        }
    });
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
