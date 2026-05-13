using FluentValidation;

namespace Procurement.Application.Features.CategoryMaster.Commands.UpdateCategoryRequest;

public class UpdateCategoryRequestCommandValidator : AbstractValidator<UpdateCategoryRequestCommand>
{
    public UpdateCategoryRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Category ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
    }
}  