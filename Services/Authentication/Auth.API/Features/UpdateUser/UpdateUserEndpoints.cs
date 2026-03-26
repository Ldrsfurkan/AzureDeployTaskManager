using BuildingBlocks.Contracts.EmployeeContracts;
using Carter;

namespace Auth.API.Features.UpdateUser;

public class UpdateUserRoleEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/role", async (UpdateUserRoleRequest request, UpdateUserRoleHandler handler) =>
        {
            var command = new UpdateUserRoleCommand(request.UserId, request.Role);
            var result = await handler.HandleAsync(command);

            return result.IsSuccess ? Results.Ok() : Results.BadRequest();
        });
    }
}