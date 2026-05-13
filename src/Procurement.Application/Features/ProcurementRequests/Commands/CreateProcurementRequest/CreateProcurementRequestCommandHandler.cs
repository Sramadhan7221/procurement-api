using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Commands.CreateProcurementRequest;

public class CreateProcurementRequestCommandHandler
    : IRequestHandler<CreateProcurementRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTimeService _dateTimeService;

    public CreateProcurementRequestCommandHandler(
        IApplicationDbContext context,
        IDateTimeService dateTimeService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
    }

    public async Task<Result<Guid>> Handle(
        CreateProcurementRequestCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.CreatedByUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(nameof(user), request.CreatedByUserId);

        if (user.Role.Name != "Staff")
            throw new DomainException("Only Staff users can create procurement requests.");

        var items = request.Items.Select(i => new ProcurementItem
        {
            ItemName = i.ItemName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList();

        var procurementRequest = new ProcurementRequest
        {
            Title = request.Title,
            Description = request.Description,
            CreatedByUserId = request.CreatedByUserId,
            CreatedAt = _dateTimeService.UtcNow,
            TotalPrice = items.Sum(i => i.SubTotal),
            Items = items
        };

        await _context.ProcurementRequests.AddAsync(procurementRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(procurementRequest.Id, "Procurement request created successfully.");
    }
}
