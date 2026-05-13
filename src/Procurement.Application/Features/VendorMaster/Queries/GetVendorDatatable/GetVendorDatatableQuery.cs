using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.VendorMaster.Queries.GetVendorDatatable;

public record GetVendorDatatableQuery(DatatableRequest Request)
    : IRequest<Result<DatatableResponse<VendorViewModel>>>;
