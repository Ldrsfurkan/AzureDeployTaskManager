using Auth.API.Entities;
using Marten;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Shared.Events;

namespace Auth.API.Features.Register;

public record RegisterUserCommand(string Username, string Password);
public record RegisterUserResult(int Id, string Username);

public class RegisterUserHandler(IDocumentSession documentSession, IPublishEndpoint publishEndpoint)
{
    public async Task<RegisterUserResult?> HandleAsync(RegisterUserCommand command)
    {
        var existingUser = await documentSession.Query<User>()
            .FirstOrDefaultAsync(u => u.Username == command.Username);

        if (existingUser is not null)
            return null;

        var user = new User
        {
            Username = command.Username,
            Role = "None"
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, command.Password);

        // Save to Marten (PostgreSQL)
        documentSession.Store(user);
        await documentSession.SaveChangesAsync();

        // Publish Event to RabbitMQ
        var integrationEvent = new UserRegisteredIntegrationEvent(user.Id, user.Username);
        await publishEndpoint.Publish(integrationEvent);

        return new RegisterUserResult(user.Id, user.Username);
    }
}