using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.VendorMaster.Commands.DeleteVendor;

public class DeleteVendorCommandHandler
    : IRequestHandler<DeleteVendorCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public DeleteVendorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        DeleteVendorCommand request,
        CancellationToken cancellationToken)
    {
        var vendor = await _context.Vendors
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (vendor is null)
            throw new NotFoundException(nameof(vendor), request.Id);

        vendor.IsDeleted = true;
        vendor.DeletedAt = DateTime.UtcNow;

        _context.Vendors.Update(vendor);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(vendor.Id, "Vendor deleted successfully.");
    }
}
