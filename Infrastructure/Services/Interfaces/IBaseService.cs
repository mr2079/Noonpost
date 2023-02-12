namespace Infrastructure.Services.Interfaces;

public interface IBaseService
{
    Task<bool> DeleteArticleImageFile(string fileName);
}
