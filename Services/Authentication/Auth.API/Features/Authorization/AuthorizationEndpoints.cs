using Auth.API.Features.Authorization;
using Carter;
public class AuthorizeUserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/authorize", async (AuthorizeUserCommand command, AuthorizationHandler handler) =>
        {
            var result = await handler.HandleAsync(command);

            if (!result.IsSuccess)
                return Results.BadRequest("User could not be found.");

            return Results.Ok(result);
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"));
    }
}

