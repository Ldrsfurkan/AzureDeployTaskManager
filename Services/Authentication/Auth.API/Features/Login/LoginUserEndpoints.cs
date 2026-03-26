using Carter;

namespace Auth.API.Features.Login;

public class LoginUserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (LoginUserCommand command, LoginUserHandler handler) =>
        {
            var result = await handler.HandleAsync(command);

            if (result is null)
                return Results.Unauthorized();

            return Results.Ok(result);
        });
    }
}
