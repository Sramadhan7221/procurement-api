using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.CategoryMaster.Queries.GetCategoryByName;

public class GetCategoryByNameQueryHandler
    : IRequestHandler<GetCategoryByNameQuery, Result<CategoryDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    async Task<Result<CategoryDetailDto>> IRequestHandler<GetCategoryByNameQuery, Result<CategoryDetailDto>>.Handle(
        GetCategoryByNameQuery request,
        CancellationToken cancellationToken)
    {
        var Category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (Category is null)
            throw new NotFoundException(nameof(Category), request.Name);

        var CategoryDetail = new CategoryDetailDto
        {
            Id = Category.Id,
            Name = Category.Name
        };

        return Result<CategoryDetailDto>.Success(CategoryDetail, "Category retrieved successfully.");
    }
}
