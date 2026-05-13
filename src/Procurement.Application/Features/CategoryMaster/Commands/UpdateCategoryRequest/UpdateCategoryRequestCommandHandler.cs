using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.CategoryMaster.Commands.UpdateCategoryRequest;

public class UpdateCategoryRequestCommandHandler
    : IRequestHandler<UpdateCategoryRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        UpdateCategoryRequestCommand request,
        CancellationToken cancellationToken)
    {
        var Category = await _context.Categories
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if(Category == null)
        {
            throw new NotFoundException($"Category with ID {request.Id} not found.", nameof(Category));
        }
        
        Category.Name = request.Name;

        _context.Categories.Update(Category);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(Category.Id, "Category updated successfully.");
    }
}