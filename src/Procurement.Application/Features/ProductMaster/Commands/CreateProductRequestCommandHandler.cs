using MediatR;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;

namespace Procurement.Application.Features.ProductMaster.Commands;

public class CreateProductRequestCommandHandler
    : IRequestHandler<CreateProductRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateProductRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        CreateProductRequestCommand request,
        CancellationToken cancellationToken)
    {
        var product = new Product
        {
            SKU = request.SKU,
            Name = request.Name,
            CategoryId = request.CategoryId,
            BasePrice = request.BasePrice,
            UoM = request.UoM,
            MetaData = request.MetaData ?? "{}",
            VendorId = request.VendorId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(product.Id, "Product created successfully.");
    }
}
