using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.VendorMaster.Queries.GetVendorById;

public record GetVendorByIdQuery(Guid Id) : IRequest<Result<VendorDetailDto>>;
