using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.VendorMaster.Commands.DeleteVendor;

public record DeleteVendorCommand(Guid Id) : IRequest<Result<Guid>>;
