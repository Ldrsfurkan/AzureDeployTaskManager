using BuildingBlocks.Contracts.EmployeeContracts;

namespace Duty.API.Employees.GetEmployeeByUserId;
public record GetEmployeeByUserIdResponse(EmployeeDto Employee);

public class GetEmployeeByUserIdEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/employees/by-user/{userId}", async (int userId, ISender sender) =>
        {
            var result = await sender.Send(new GetEmployeeByUserIdQuery(userId));
            var response = result.Adapt<GetEmployeeByUserIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetEmployeeByUserId")
        .Produces<GetEmployeeByUserIdResponse>(StatusCodes.Status200OK);
    }
}