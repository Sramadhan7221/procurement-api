using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProductMaster.Queries.GetProductDatatable;

public record GetProductDatatableQuery(ProductDatatableRequest Request)
    : IRequest<Result<DatatableResponse<ProductViewModel>>>;
