using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.VendorMaster.Commands;

public class CreateVendorRequestCommandHandler
    : IRequestHandler<CreateVendorRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateVendorRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        CreateVendorRequestCommand request,
        CancellationToken cancellationToken)
    {
        var vendor = new Procurement.Domain.Entities.Vendor
        {
            Name = request.Name,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            Address = request.Address
        };

        _context.Vendors.Add(vendor);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(vendor.Id, "Vendor created successfully.");
    }
}