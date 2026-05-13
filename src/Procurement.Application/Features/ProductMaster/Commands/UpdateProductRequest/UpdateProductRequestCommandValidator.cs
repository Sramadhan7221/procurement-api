using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Interfaces;

namespace Procurement.Application.Features.ProductMaster.Commands.UpdateProductRequest;

public class UpdateProductRequestCommandValidator : AbstractValidator<UpdateProductRequestCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductRequestCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(50).WithMessage("SKU must not exceed 50 characters.")
            .MustAsync(BeUniqueSkuExcludingSelf).WithMessage("SKU already exists.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("Base price must be greater than 0.");
    }

    private async Task<bool> BeUniqueSkuExcludingSelf(
        UpdateProductRequestCommand command,
        string sku,
        CancellationToken cancellationToken)
    {
        return !await _context.Products
            .AnyAsync(p => p.SKU == sku && p.Id != command.Id, cancellationToken);
    }
}
