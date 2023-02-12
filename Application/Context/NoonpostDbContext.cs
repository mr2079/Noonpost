using Domain.Entites.Article;
using Domain.Entites.Comment;
using Domain.Entites.User;
using Microsoft.EntityFrameworkCore;

namespace Application.Context;

public class NoonpostDbContext : DbContext
{
    public NoonpostDbContext(DbContextOptions<NoonpostDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleImage> ArticleImages { get; set; }
    public DbSet<Comment> Comments { get; set; }

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


        base.OnModelCreating(modelBuilder);
    }
}
