using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.CategoryMaster.Queries.GetCategoryDatatable;

public record GetCategoryDatatableQuery(DatatableRequest Request)
    : IRequest<Result<DatatableResponse<CategoryViewModel>>>;
