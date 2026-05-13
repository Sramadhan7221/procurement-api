using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.CategoryMaster.Commands;

public record CreateCategoryRequestCommand : IRequest<Result<Guid>>
{
    public string Name { get; init; } = string.Empty;
}