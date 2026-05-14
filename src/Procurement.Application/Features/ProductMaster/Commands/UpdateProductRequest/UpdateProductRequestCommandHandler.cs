using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProductMaster.Commands.UpdateProductRequest;

public class UpdateProductRequestCommandHandler
    : IRequestHandler<UpdateProductRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileService _fileService;

    public UpdateProductRequestCommandHandler(IApplicationDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Result<Guid>> Handle(
        UpdateProductRequestCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
            throw new NotFoundException(nameof(product), request.Id);

        string existingImageUrl = string.Empty;
        if (!string.IsNullOrEmpty(product.MetaData))
        {
            var existing = JsonSerializer.Deserialize<JsonElement>(product.MetaData);
            if (existing.TryGetProperty("imageUrl", out var img))
                existingImageUrl = img.GetString() ?? string.Empty;
        }

        string imageUrl = existingImageUrl;
        if (request.ProductImage is not null)
            imageUrl = await _fileService.UploadFileAsync(request.ProductImage, "products");

        product.SKU = request.SKU;
        product.Name = request.Name;
        product.CategoryId = request.CategoryId;
        product.BasePrice = request.BasePrice;
        product.UoM = request.UoM;
        product.MetaData = JsonSerializer.Serialize(new
        {
            detail = request.Detail ?? string.Empty,
            imageUrl
        });
        product.VendorId = request.VendorId;
        product.UpdatedAt = DateTime.UtcNow;

        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(product.Id, "Product updated successfully.");
    }
}
