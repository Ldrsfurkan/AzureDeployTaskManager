namespace Duty.API.Duties.CreateDuty;

public record CreateDutyCommand(string Description, string MailDescription, int Priority, string Status, int ClientId, int? AssignedEmployeeId)
   : ICommand<CreateDutyResult>;
public record CreateDutyResult(int Id);
internal class CreateDutyCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateDutyCommand, CreateDutyResult>
{
    public async Task<CreateDutyResult> Handle(CreateDutyCommand command, CancellationToken cancellationToken)
    {             
        var duty = new DutyEntity
        {
            Description = command.Description,
            MailDescription = command.MailDescription,
            Priority = command.Priority,
            Status = command.Status,
            ClientId = command.ClientId,
            AssignedEmployeeId = command.AssignedEmployeeId
        };

        //save to database
         session.Store(duty);

        var log = new AuditLog
        {
            Action = "New Task Created",
            Details = $"Task ID {duty.Id}, {duty.Description} created."
        };
        session.Store(log);

        await session.SaveChangesAsync(cancellationToken);
         
        //return result
        return new CreateDutyResult(duty.Id);
    }
}
