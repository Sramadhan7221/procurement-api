using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.CategoryMaster.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler
    : IRequestHandler<DeleteCategoryCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (category is null)
            throw new NotFoundException(nameof(category), request.Id);

        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;

        _context.Categories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(category.Id, "Category deleted successfully.");
    }
}
