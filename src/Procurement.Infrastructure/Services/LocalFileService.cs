using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Procurement.Application.Interfaces;

namespace Procurement.Infrastructure.Services;

public class LocalFileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalFileService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        _env = env;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
        var uploadsRoot = Path.Combine(webRoot, "uploads", folder);

        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadsRoot, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        var request = _httpContextAccessor.HttpContext!.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        return $"{baseUrl}/uploads/{folder}/{fileName}";
    }
}
