using Application.Context;
using Domain.Entites.Comment;
using Domain.Entites.User;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Services;

public class CommentService : ICommentService
{
    private readonly NoonpostDbContext _context;

    public CommentService(NoonpostDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateComment(CreateCommentViewModel userComment)
    {
        var comment = new Comment()
        {
            ArticleId = userComment.ArticleId,
            UserId = userComment.UserId,
            Text = userComment.CommentText,
            IsAccepted = true,
            CreateDate = DateTime.Now,
        };

        try
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> CreateComment(Guid articleId, Guid parentId, Guid userId, string text)
    {
        var comment = new Comment()
        {
            UserId = userId,
            ArticleId = articleId,
            ParentId = parentId,
            Text = text
        };

        try
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<Guid> EditComment(Guid commentId, string text)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null) return Guid.Empty;
        comment.Text = text;
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();

        return comment.ArticleId;
    }

    public async Task<Guid> DeleteComment(Guid commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null) return Guid.Empty;
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return comment.ArticleId;
    }
}
