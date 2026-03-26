using Shared.Events;
namespace Duty.API.Consumers;

public class UserRoleUpdatedConsumer(IDocumentSession documentSession,
    ILogger<UserRoleUpdatedConsumer> logger) : IConsumer<UserRoleUpdatedEvent>
{
    public async Task Consume(ConsumeContext<UserRoleUpdatedEvent> context)
    {
        var message = context.Message;

        var employee = await documentSession
            .Query<Employee>()
            .FirstOrDefaultAsync(e => e.UserId == message.UserId, context.CancellationToken);

        if (employee is null)
        {
            logger.LogWarning("Employee with UserId {UserId} was not found. Creating a new employee record.", message.UserId);

            employee = new Employee
            {
                // We leave the 'Id' empty/default so Marten can auto-generate the Primary Key
                UserId = message.UserId,
                UserRole = message.NewRole
            };
        }
        else
        {
            employee.UserRole = message.NewRole;
        }

        documentSession.Store(employee);
        await documentSession.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation("Successfully processed role update for UserId: {UserId}. New Role: {NewRole}", message.UserId, message.NewRole);
    }
}