using Carter;
using MediatR;

namespace Auth.API.Features.GetUsers;

public class GetUsersEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Inject GetUsersHandler directly
        app.MapGet("/users", async (GetUsersHandler handler) =>
        {
            var result = await handler.HandleAsync();
            return Results.Ok(result);
        });

    }
}
