using MediatR;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;

namespace Procurement.Application.Features.CategoryMaster.Commands;

public class CreateCategoryRequestCommandHandler
    : IRequestHandler<CreateCategoryRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        CreateCategoryRequestCommand request,
        CancellationToken cancellationToken)
    {
        var Category = new Domain.Entities.Category
        {
            Name = request.Name
        };

        _context.Categories.Add(Category);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(Category.Id, "Category created successfully.");
    }
}