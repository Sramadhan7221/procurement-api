using FluentValidation;

namespace Procurement.Application.Features.ProcurementRequests.Commands.CreateProcurementRequest;

public class CreateProcurementRequestCommandValidator : AbstractValidator<CreateProcurementRequestCommand>
{
    public CreateProcurementRequestCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.CreatedByUserId)
            .NotEmpty().WithMessage("CreatedByUserId is required.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ItemName)
                .NotEmpty().WithMessage("Item name is required.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            item.RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
        });
    }
}
