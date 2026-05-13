using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProductMaster.Commands.UpdateProductRequest;

public record UpdateProductRequestCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; init; }
    public string SKU { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public Guid CategoryId { get; init; }
    public decimal BasePrice { get; init; }
    public string UoM { get; init; } = string.Empty;
    public string? MetaData { get; init; }
    public Guid? VendorId { get; init; }
}
