using FluentValidation;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ConfirmGoodsReceipt;

public class ConfirmGoodsReceiptCommandValidator : AbstractValidator<ConfirmGoodsReceiptCommand>
{
    public ConfirmGoodsReceiptCommandValidator()
    {
        RuleFor(x => x.ProcurementId)
            .NotEmpty().WithMessage("ProcurementId is required.");

        RuleFor(x => x.ReceivedByUserId)
            .NotEmpty().WithMessage("ReceivedByUserId is required.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one goods receipt item is required.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProcurementItemId)
                .NotEmpty().WithMessage("ProcurementItemId is required.");

            item.RuleFor(i => i.ReceivedQuantity)
                .GreaterThan(0).WithMessage("ReceivedQuantity must be greater than 0.");
        });

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.");
    }
}
