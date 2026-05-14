using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestDatatable;

public class GetProcurementRequestDatatableQueryHandler
    : IRequestHandler<GetProcurementRequestDatatableQuery, Result<DatatableResponse<ProcurementRequestListItemDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetProcurementRequestDatatableQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DatatableResponse<ProcurementRequestListItemDto>>> Handle(
        GetProcurementRequestDatatableQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(nameof(user), request.UserId);

        var roleName = user.Role.Name;
        if (roleName != "Admin" && roleName != "Manager")
            return Result<DatatableResponse<ProcurementRequestListItemDto>>.Failure(
                "Only Admin and Manager users are allowed to access the procurement request list.");

        var query = _context.ProcurementRequests
            .Include(r => r.CreatedBy)
            .Where(r => !r.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(r =>
                r.Title.ToLower().Contains(search) ||
                r.Description.ToLower().Contains(search) ||
                r.CreatedBy!.Name.ToLower().Contains(search));
        }

        var totalRecords = await query.CountAsync(cancellationToken);

        var data = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip(request.Start)
            .Take(request.Length > 0 ? request.Length : 10)
            .Select(r => new ProcurementRequestListItemDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                TotalPrice = r.TotalPrice,
                Status = r.Status.ToString(),
                CreatedByUserId = r.CreatedByUserId,
                CreatedByName = r.CreatedBy!.Name,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result<DatatableResponse<ProcurementRequestListItemDto>>.Success(new DatatableResponse<ProcurementRequestListItemDto>
        {
            Draw = request.Draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        });
    }
}
