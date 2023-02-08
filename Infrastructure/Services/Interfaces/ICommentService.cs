using Infrastructure.ViewModels;

namespace Infrastructure.Services.Interfaces;

public interface ICommentService
{
    Task<bool> CreateComment(CreateCommentViewModel userComment);
    Task<bool> CreateComment(Guid articleId, Guid parentId, Guid userId, string text);
    Task<Guid> EditComment(Guid commentId, string text);
    Task<Guid> DeleteComment(Guid commentId);
}
 