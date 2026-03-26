using Auth.API.Entities;
using Marten;

namespace Auth.API.Features.DeleteUser;

public record DeleteUserCommand(int UserId);
public record DeleteUserResult(bool IsSuccess);

public class DeleteUserHandler(IDocumentSession session)
{
    public async Task<DeleteUserResult> HandleAsync(DeleteUserCommand command)
    {
        // Delete the user directly by ID
        session.Delete<User>(command.UserId);
        await session.SaveChangesAsync();

        return new DeleteUserResult(true);
    }
}
