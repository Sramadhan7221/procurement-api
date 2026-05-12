using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestById;

public class GetProcurementRequestByIdQueryHandler
    : IRequestHandler<GetProcurementRequestByIdQuery, Result<ProcurementRequestDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProcurementRequestByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ProcurementRequestDetailDto>> Handle(
        GetProcurementRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.ProcurementRequests
            .Include(r => r.Items)
            .Include(r => r.CreatedBy)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(entity), request.Id);

        var dto = new ProcurementRequestDetailDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            TotalPrice = entity.TotalPrice,
            Status = entity.Status.ToString(),
            CreatedByUserId = entity.CreatedByUserId,
            CreatedByName = entity.CreatedBy?.Name ?? string.Empty,
            CreatedAt = entity.CreatedAt,
            Items = entity.Items.Select(i => new ProcurementItemDto
            {
                Id = i.Id,
                ItemName = i.ItemName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                SubTotal = i.SubTotal
            }).ToList()
        };

        return Result<ProcurementRequestDetailDto>.Success(dto);
    }
}
