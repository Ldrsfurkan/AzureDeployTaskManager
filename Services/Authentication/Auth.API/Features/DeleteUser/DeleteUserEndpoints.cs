using Carter;

namespace Auth.API.Features.DeleteUser;

public class DeleteUserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/{id}", async (int id, DeleteUserHandler handler) =>
        {
            var command = new DeleteUserCommand(id);
            var result = await handler.HandleAsync(command);

            return result.IsSuccess ? Results.Ok() : Results.BadRequest();
        });
    }
}
