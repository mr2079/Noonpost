using Infrastructure.Generator;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class BaseService : IBaseService
{
    public async Task<string> SaveImageFile(IFormFile file, string directoryName)
    {
        try
        {
            var fileName = NameGenerator.Generate() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{directoryName}", fileName);
            using(var fs = new FileStream(filePath, FileMode.Create)) await file.CopyToAsync(fs);
            return fileName;
        }
        catch { return string.Empty; }
    }

    public Task<bool> DeleteImageFile(string fileName, string directoryName)
	{
		try
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{directoryName}", fileName);
			if (File.Exists(filePath)) File.Delete(filePath);
			return Task.FromResult(true);
		}
		catch { return Task.FromResult(false); }
	}
}
