//using BuildingBlocks.Events;
using Shared.Events;

namespace Duty.API.Consumers;

public class UserRegisteredEventConsumer(IDocumentSession documentSession, 
    ILogger<UserRegisteredEventConsumer> logger) : IConsumer<UserRegisteredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        var message = context.Message;

        var newEmployee = new Employee
        {
          //  Id = Guid.NewGuid(),
            UserId = message.UserId,
            Name = message.Username
        };

        // Saving to Duty.API's own database using Marten
        //documentSession.Store(newEmployee); //artık burdan almıyoruz web application üzerinden update olarak alıyoruz
        await documentSession.SaveChangesAsync();
        logger.LogInformation("Employee successfully saved to Marten database. Employee ID: {EmployeeId}", newEmployee.Id);
    }
}
