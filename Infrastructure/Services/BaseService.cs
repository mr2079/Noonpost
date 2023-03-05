using Infrastructure.Converter;
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
            using (var fs = new FileStream(filePath, FileMode.Create)) await file.CopyToAsync(fs);

            if (string.Equals(directoryName, "articles"))
            {
                var t400path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\articles\\thumb400", fileName);
                var t600path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\articles\\thumb600", fileName);
                var ir = new ImageConverter();
                ir.Image_resize(filePath, t400path, 400);
                ir.Image_resize(filePath, t600path, 600);
            }

            if (string.Equals(directoryName, "users"))
            {
                var t60path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\users\\thumb60", fileName);
                var t200path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\users\\thumb200", fileName);
                var ir = new ImageConverter();
                ir.Image_resize(filePath, t60path, 60);
                ir.Image_resize(filePath, t200path, 200);
            }

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

            if (string.Equals(directoryName, "articles"))
            {
                var t400path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\articles\\thumb400", fileName);
                var t600path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\articles\\thumb600", fileName);
                if (File.Exists(t400path)) File.Delete(t400path);
                if (File.Exists(t600path)) File.Delete(t600path);
            }

            if (string.Equals(directoryName, "users"))
            {
                var t60path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\users\\thumb60", fileName);
                var t200path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\users\\thumb200", fileName);
                if (File.Exists(t60path)) File.Delete(t60path);
                if (File.Exists(t200path)) File.Delete(t200path);
            }

            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }
}
