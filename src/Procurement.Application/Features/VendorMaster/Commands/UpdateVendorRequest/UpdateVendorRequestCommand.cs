using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.VendorMaster.Commands.UpdateVendorRequest;

public record UpdateVendorRequestCommand : IRequest<Result<Guid>>
{
    public Guid Id { get;set; }
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}