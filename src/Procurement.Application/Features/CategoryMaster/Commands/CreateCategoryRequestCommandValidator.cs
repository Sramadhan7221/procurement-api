using FluentValidation;

namespace Procurement.Application.Features.CategoryMaster.Commands;

public class CreateCategoryRequestCommandValidator : AbstractValidator<CreateCategoryRequestCommand>
{
    public CreateCategoryRequestCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
    }
}  