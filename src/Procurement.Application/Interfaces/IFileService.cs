using Microsoft.AspNetCore.Http;

namespace Procurement.Application.Interfaces;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string folder);
}
