using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Procurement.Application.Interfaces;

namespace Procurement.Infrastructure.Services;

public class LocalFileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    public LocalFileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", folder);

        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadsRoot, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/{folder}/{fileName}";
    }
}
