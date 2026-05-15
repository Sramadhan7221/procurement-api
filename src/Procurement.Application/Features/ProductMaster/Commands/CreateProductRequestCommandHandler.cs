using System.Text.Json;
using MediatR;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;

namespace Procurement.Application.Features.ProductMaster.Commands;

public class CreateProductRequestCommandHandler
    : IRequestHandler<CreateProductRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileService _fileService;

    public CreateProductRequestCommandHandler(IApplicationDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Result<Guid>> Handle(
        CreateProductRequestCommand request,
        CancellationToken cancellationToken)
    {
        string? imageUrl = null;
        if (request.ProductImage is not null)
            imageUrl = await _fileService.UploadFileAsync(request.ProductImage, "products");

        var metaData = JsonSerializer.Serialize(new
        {
            detail = request.Detail ?? string.Empty,
            imageUrl = imageUrl ?? string.Empty
        });

        var product = new Product
        {
            SKU = request.SKU,
            Name = request.Name,
            CategoryId = request.CategoryId,
            BasePrice = request.BasePrice,
            UoM = request.UoM,
            MetaData = metaData,
            VendorId = request.VendorId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(product.Id, "Product created successfully.");
    }
}
