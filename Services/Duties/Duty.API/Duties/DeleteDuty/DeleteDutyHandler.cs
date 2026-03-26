using FluentValidation;

namespace Duty.API.Duties.DeleteDuty;
public record DeleteDutyCommand(int Id) : ICommand<DeleteDutyResult>;
public record DeleteDutyResult(bool IsSuccess);

public class DeleteDutyCommandValidator : AbstractValidator<DeleteDutyCommand>
{
    public DeleteDutyCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Duty ID is required");
    }
}

internal class DeleteDutyCommandHandler
    (IDocumentSession session)
    : ICommandHandler<DeleteDutyCommand, DeleteDutyResult>
{
    public async Task<DeleteDutyResult> Handle(DeleteDutyCommand command, CancellationToken cancellationToken)
    {
        session.Delete<DutyEntity>(command.Id);

        var log = new AuditLog
        {
            Action = " A Task Deleted",
            Details = $"Task ID {command.Id} deleted."
        };
        session.Store(log);

        await session.SaveChangesAsync(cancellationToken);

        return new DeleteDutyResult(true);
    }
}

