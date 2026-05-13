using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProductMaster.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<Result<Guid>>;
