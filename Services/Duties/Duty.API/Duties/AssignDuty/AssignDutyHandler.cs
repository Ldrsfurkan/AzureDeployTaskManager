using FluentValidation;

namespace Duty.API.Duties.AssignDuty;

public record AssignDutyCommand(int Id, int UserId) : ICommand<AssignDutyResult>;
public record AssignDutyResult(bool IsSuccess);

public class AssignDutyCommandValidator : AbstractValidator<AssignDutyCommand>
{
    public AssignDutyCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Duty ID is required");
        RuleFor(command => command.UserId).NotEmpty().WithMessage("Employee ID is required for assignment");
    }
}

internal class AssignDutyCommandHandler
    (IDocumentSession session)
    : ICommandHandler<AssignDutyCommand, AssignDutyResult>
{
    public async Task<AssignDutyResult> Handle(AssignDutyCommand command, CancellationToken cancellationToken)
    {
        var duty = await session.LoadAsync<DutyEntity>(command.Id, cancellationToken);

        if (duty is null)
        {
            throw new DutyNotFoundException(command.Id);
        }

        duty.AssignedEmployeeId = command.UserId;
        duty.UpdatedAt = DateTime.UtcNow;

        session.Update(duty);
        await session.SaveChangesAsync(cancellationToken);

        return new AssignDutyResult(true);
    }
}