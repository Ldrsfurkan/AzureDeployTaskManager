using Auth.API.Entities;
using BuildingBlocks.CQRS;
using Marten;

namespace Auth.API.Features.UpdateUser;

public record UpdateUserRoleCommand(int UserId, string Role) : ICommand<UpdateUserRoleResult>;
public record UpdateUserRoleResult(bool IsSuccess);

public class UpdateUserRoleHandler(IDocumentSession session)
{
    public async Task<UpdateUserRoleResult> HandleAsync(UpdateUserRoleCommand command)
    {
        var user = await session.LoadAsync<User>(command.UserId);

        if (user == null)
            return new UpdateUserRoleResult(false);

        user.Role = command.Role;

        session.Update(user);
        await session.SaveChangesAsync();

        return new UpdateUserRoleResult(true);
    }
}