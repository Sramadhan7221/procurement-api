using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.CategoryMaster.Commands.UpdateCategoryRequest;

public record UpdateCategoryRequestCommand : IRequest<Result<Guid>>
{
    public Guid Id { get;set; }
    public string Name { get; set; } = string.Empty;
}