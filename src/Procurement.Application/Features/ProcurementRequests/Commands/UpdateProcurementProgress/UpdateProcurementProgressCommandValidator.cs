using FluentValidation;
using Procurement.Domain.Enums;

namespace Procurement.Application.Features.ProcurementRequests.Commands.UpdateProcurementProgress;

public class UpdateProcurementProgressCommandValidator
    : AbstractValidator<UpdateProcurementProgressCommand>
{
    private static readonly ProcurementStatus[] AllowedStatuses =
    [
        ProcurementStatus.InOrderByAdmin,
        ProcurementStatus.OrderReceived,
        ProcurementStatus.Completed
    ];

    public UpdateProcurementProgressCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Procurement request Id is required.");

        RuleFor(x => x.AdminUserId)
            .NotEmpty().WithMessage("AdminUserId is required.");

        RuleFor(x => x.NewStatus)
            .Must(s => AllowedStatuses.Contains(s))
            .WithMessage($"NewStatus must be one of: {string.Join(", ", AllowedStatuses.Select(s => s.ToString()))}.");
    }
}
