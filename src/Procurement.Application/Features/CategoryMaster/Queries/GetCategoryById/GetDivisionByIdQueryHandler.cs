using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.CategoryMaster.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler
    : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    async Task<Result<CategoryDetailDto>> IRequestHandler<GetCategoryByIdQuery, Result<CategoryDetailDto>>.Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var Category = await _context.Categories
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (Category is null)
            throw new NotFoundException(nameof(Category), request.Id);

        var CategoryDetail = new CategoryDetailDto
        {
            Name = Category.Name
        };

        return Result<CategoryDetailDto>.Success(CategoryDetail, "Category retrieved successfully.");
    }
}
