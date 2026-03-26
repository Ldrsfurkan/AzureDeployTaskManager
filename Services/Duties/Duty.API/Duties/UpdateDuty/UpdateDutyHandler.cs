using FluentValidation;

namespace Duty.API.Duties.UpdateDuty;

public record UpdateDutyCommand(int Id, string? Description, string? MailDescription, int? Priority, string? Status, int? AssignedEmployeeId,int? ClientId)
    : ICommand<UpdateDutyResult>;
public record UpdateDutyResult(bool IsSuccess);

public class UpdateDutyCommandValidator : AbstractValidator<UpdateDutyCommand>
{
    public UpdateDutyCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Duty ID is required");
    }
}

internal class UpdateDutyCommandHandler
    (IDocumentSession session)
    : ICommandHandler<UpdateDutyCommand, UpdateDutyResult>
{
    public async Task<UpdateDutyResult> Handle(UpdateDutyCommand command, CancellationToken cancellationToken)
    {
        var duty = await session.LoadAsync<DutyEntity>(command.Id, cancellationToken);

        if (duty is null)
        {
            throw new DutyNotFoundException(command.Id);
        }

        duty.Description = command.Description ?? duty.Description;
        duty.MailDescription = command.MailDescription ?? duty.MailDescription;
        duty.Priority = command.Priority ?? duty.Priority;
        duty.Status = command.Status ?? duty.Status;

        duty.AssignedEmployeeId = command.AssignedEmployeeId;
        duty.ClientId = command.ClientId;

        duty.UpdatedAt = DateTime.UtcNow;

        session.Update(duty);

        var log = new AuditLog
        {
            Action = "Task Status Updated",
            Details = $"Task ID {duty.Id}, {duty.Description} status was changed to {duty.Status}."
        };
        session.Store(log);

        await session.SaveChangesAsync(cancellationToken);

        return new UpdateDutyResult(true);
    }
}
