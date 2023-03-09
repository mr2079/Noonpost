using Domain.Entites.Article;
using Domain.Entites.Category;
using Domain.Entites.Comment;
using Domain.Entites.User;
using Microsoft.EntityFrameworkCore;

namespace Application.Context;

public class NoonpostDbContext : DbContext
{
    public NoonpostDbContext(DbContextOptions<NoonpostDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<ArticleImage> ArticleImages { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>()
            .Property(u => u.Id).HasColumnName("UserId");
        modelBuilder.Entity<User>()
            .Property(u => u.CreateDate).HasColumnName("RegisterDate");

        // Article
        modelBuilder.Entity<Article>()
            .Property(a => a.Id).HasColumnName("ArticleId");

        // Article Image
        modelBuilder.Entity<ArticleImage>()
            .Property(a => a.Id).HasColumnName("ArticleImageId");

        // Comment
        modelBuilder.Entity<Comment>()
            .Property(c => c.Id).HasColumnName("CommentId");

        // Category
        modelBuilder.Entity<Category>()
            .Property(c => c.Id).HasColumnName("CategoryId");

        // Add Admin to User Entity in Initial Migration
        modelBuilder.Entity<User>()
            .HasData(
                new User()
                {
                    Id = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    CId = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssffff")),
                    FirstName = "محمدعلی",
                    LastName = "خوشدونی",
                    Mobile = "09198585873",
                    Role = "Admin",
                    Password = "B3-E6-3E-3A-88-45-B1-E3-66-8C-9A-F7-56-BF-F6-9F",
                }
            );


        base.OnModelCreating(modelBuilder);
    }
}
