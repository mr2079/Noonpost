using Application.Context;
using Domain.Entites.Comment;
using Domain.Entites.User;
using Infrastructure.Converter;
using Infrastructure.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;
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
            CId = DateTime.Now.ToTimeStamp(),
            ArticleId = userComment.ArticleId,
            UserId = userComment.UserId,
            Text = userComment.CommentText,
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
            CId = DateTime.Now.ToTimeStamp(),
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

    public async Task DeleteComment(Guid commentId)
    {
        var comments = await _context.Comments
            .Include(c => c.Replies).ToListAsync();
        var flatten = Flatten(comments.Where(c => Equals(c.Id, commentId)));
        _context.Comments.RemoveRange(flatten);
        await _context.SaveChangesAsync();
        await Task.CompletedTask;
    }

    private IEnumerable<Comment> Flatten(IEnumerable<Comment> comments)
        => comments.SelectMany(c => Flatten(c.Replies)).Concat(comments);
}
