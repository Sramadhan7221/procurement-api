using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.VendorMaster.Queries.GetVendorList;

public class GetVendorListQueryHandler
    : IRequestHandler<GetVendorListQuery, Result<List<GetVendorListDetailDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetVendorListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    async Task<Result<List<GetVendorListDetailDto>>> IRequestHandler<GetVendorListQuery, Result<List<GetVendorListDetailDto>>>.Handle(
        GetVendorListQuery request,
        CancellationToken cancellationToken)
    {
        var vendor = _context.Vendors
            .Where(v => v.IsDeleted == false);

        if(!string.IsNullOrEmpty(request.searchQuery))
        {
            vendor = vendor.Where(v => v.Name.Contains(request.searchQuery, StringComparison.CurrentCultureIgnoreCase));
        }
        var vendorList = await vendor
            .Select(v => new GetVendorListDetailDto
            {
                Id = v.Id,
                Name = v.Name
            })
            .ToListAsync(cancellationToken);

        return Result<List<GetVendorListDetailDto>>.Success(vendorList, "Vendor retrieved successfully.");
    }
}
