namespace Duty.API.Duties.AssignDuty;

public record AssignDutyRequest(int EmployeeId);
public record AssignDutyResponse(bool IsSuccess);

public class AssignDutyEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/duties/{id:int}/assign",
            async (int id, AssignDutyRequest request, ISender sender) =>
            {
                var command = new AssignDutyCommand(id, request.EmployeeId);

                var result = await sender.Send(command);

                var response = result.Adapt<AssignDutyResponse>();

                return Results.Ok(response);
            })
            .WithName("AssignDuty")
            .Produces<AssignDutyResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Assign Employee to Duty")
            .WithDescription("Assigns a specific employee to an existing duty")
            .RequireAuthorization(policy => policy.RequireRole("Admin"));  //Admin role assign yapabilir
    }
}