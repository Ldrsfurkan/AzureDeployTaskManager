using Auth.API.Entities;
using Marten;
using MassTransit;
using Shared.Events;

namespace Auth.API.Features.Authorization;

public record class AuthorizeUserCommand(int Id, string Role);
public record class AuthorizeUserResult(bool IsSuccess);

    public class AuthorizationHandler(IDocumentSession documentSession, IPublishEndpoint publishEndpoint)
    {
        public async Task<AuthorizeUserResult> HandleAsync(AuthorizeUserCommand command, CancellationToken cancellationToken = default)
        {
            var user = await documentSession.LoadAsync<User>(command.Id, cancellationToken);

            if (user is null)
            {
                return new AuthorizeUserResult(false);
            }

            user.Role = command.Role;
            documentSession.Update(user);
            await documentSession.SaveChangesAsync(cancellationToken);

            var integrationEvent = new UserRoleUpdatedEvent(user.Id, user.Role);
            await publishEndpoint.Publish(integrationEvent, cancellationToken);

            return new AuthorizeUserResult(true);
        }
    }

