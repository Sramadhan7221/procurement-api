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

    public UpdateProductRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        UpdateProductRequestCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
            throw new NotFoundException(nameof(product), request.Id);

        product.SKU = request.SKU;
        product.Name = request.Name;
        product.CategoryId = request.CategoryId;
        product.BasePrice = request.BasePrice;
        product.UoM = request.UoM;
        product.MetaData = request.MetaData ?? "{}";
        product.VendorId = request.VendorId;
        product.UpdatedAt = DateTime.UtcNow;

        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(product.Id, "Product updated successfully.");
    }
}
