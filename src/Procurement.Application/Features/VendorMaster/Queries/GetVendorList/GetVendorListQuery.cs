using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.VendorMaster.Queries.GetVendorList;

public record GetVendorListQuery(string? searchQuery) : IRequest<Result<List<GetVendorListDetailDto>>>;

