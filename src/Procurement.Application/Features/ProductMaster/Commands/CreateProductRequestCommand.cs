using MediatR;
using Microsoft.AspNetCore.Http;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProductMaster.Commands;

public record CreateProductRequestCommand : IRequest<Result<Guid>>
{
    public string SKU { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public Guid CategoryId { get; init; }
    public decimal BasePrice { get; init; }
    public string UoM { get; init; } = string.Empty;
    public string? Detail { get; init; }
    public IFormFile? ProductImage { get; init; }
    public Guid? VendorId { get; init; }
}
