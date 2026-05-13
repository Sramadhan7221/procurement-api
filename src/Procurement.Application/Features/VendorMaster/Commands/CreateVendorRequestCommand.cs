using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.VendorMaster.Commands;

public record CreateVendorRequestCommand : IRequest<Result<Guid>>
{
    public string Name { get; init; } = string.Empty;
    public string ContactEmail { get; init; } = string.Empty;
    public string ContactPhone { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
}