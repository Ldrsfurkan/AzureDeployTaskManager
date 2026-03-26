namespace Duty.API.Duties.GetDutiesByEmployee;

public class GetDutiesByEmployeeEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // URL pattern expects the employeeId in the route
        app.MapGet("/duties/employee/{employeeId}", async (int employeeId, ISender sender) =>
        {
            var result = await sender.Send(new GetDutiesByEmployeeQuery(employeeId));

            var response = result.Adapt<GetDutiesResponse>();

            return Results.Ok(response);
        })
        .WithName("GetDutiesByEmployee")
        .Produces<GetDutiesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Duties By Employee")
        .WithDescription("Get Duties assigned to a specific Employee");
    }
}