using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services;

public class BaseService : IBaseService
{
	public Task<bool> DeleteArticleImageFile(string fileName)
	{
		try
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\articles", fileName);
			if (File.Exists(filePath)) File.Delete(filePath);
			return Task.FromResult(true);
		}
		catch { return Task.FromResult(false); }
	}
}
