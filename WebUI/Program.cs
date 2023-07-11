using Application.Context;
using Infrastructure.IoC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Authentication
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
#endregion

#region Database Context
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnectionString") ??
    throw new InvalidOperationException("Connection string 'DefaultConnectionString' not found.");

builder.Services.AddDbContext<NoonpostDbContext>(options =>
    options.UseSqlServer(defaultConnectionString));
#endregion

#region IoC
builder.Services.AddInfrastructureServices();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
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
