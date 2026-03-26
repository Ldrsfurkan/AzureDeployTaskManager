using Carter;

namespace Auth.API.Features.Register
{
    public class RegisterUserEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/register", async (RegisterUserCommand command, RegisterUserHandler handler) =>
            {
                var result = await handler.HandleAsync(command);

                if (result is null)
                    return Results.BadRequest("Username already exists.");

                return Results.Ok(result);
            });
        }
    }
}
