using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.VendorMaster.Commands.UpdateVendorRequest;

public class UpdateVendorRequestCommandHandler
    : IRequestHandler<UpdateVendorRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public UpdateVendorRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        UpdateVendorRequestCommand request,
        CancellationToken cancellationToken)
    {
        var vendor = await _context.Vendors
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if(vendor == null)
        {
            throw new NotFoundException($"Vendor with ID {request.Id} not found.", nameof(vendor));
        }
        
        vendor.Name = request.Name;
        vendor.ContactEmail = request.ContactEmail;
        vendor.ContactPhone = request.ContactPhone;
        vendor.Address = request.Address;

        _context.Vendors.Update(vendor);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(vendor.Id, "Vendor updated successfully.");
    }
}