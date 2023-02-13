using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Interfaces;

public interface IBaseService
{
    Task<string> SaveImageFile(IFormFile file, string directoryName);
    Task<bool> DeleteImageFile(string fileName, string directoryName);
}
