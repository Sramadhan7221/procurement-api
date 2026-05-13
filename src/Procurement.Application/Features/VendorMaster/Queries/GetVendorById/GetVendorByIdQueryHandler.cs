using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.VendorMaster.Queries.GetVendorById;

public class GetVendorByIdQueryHandler
    : IRequestHandler<GetVendorByIdQuery, Result<VendorDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetVendorByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    async Task<Result<VendorDetailDto>> IRequestHandler<GetVendorByIdQuery, Result<VendorDetailDto>>.Handle(
        GetVendorByIdQuery request,
        CancellationToken cancellationToken)
    {
        var vendor = await _context.Vendors
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (vendor is null)
            throw new NotFoundException(nameof(vendor), request.Id);

        var vendorDetail = new VendorDetailDto
        {
            Name = vendor.Name,
            ContactEmail = vendor.ContactEmail,
            ContactPhone = vendor.ContactPhone,
            Address = vendor.Address
        };

        return Result<VendorDetailDto>.Success(vendorDetail, "Vendor retrieved successfully.");
    }
}
