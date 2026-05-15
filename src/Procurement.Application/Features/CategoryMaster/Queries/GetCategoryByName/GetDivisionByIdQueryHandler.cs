using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.CategoryMaster.Queries.GetCategoryByName;

public class GetCategoryByNameQueryHandler
    : IRequestHandler<GetCategoryByNameQuery, Result<List<CategoryDetailDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    async Task<Result<List<CategoryDetailDto>>> IRequestHandler<GetCategoryByNameQuery, Result<List<CategoryDetailDto>>>.Handle(
        GetCategoryByNameQuery request,
        CancellationToken cancellationToken)
    {
        var searchTerm = string.IsNullOrEmpty(request.Name) ? string.Empty : request.Name.ToLower();

         var categories = _context.Categories
            .Where(c => c.IsDeleted == false);

        if(!string.IsNullOrEmpty(request.Name))
        {
            categories = categories.Where(c => c.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase));
        }
        var categoryList = await categories
            .Select(c => new CategoryDetailDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync(cancellationToken);

        return Result<List<CategoryDetailDto>>.Success(categoryList, "Categories retrieved successfully.");
    }
}
