namespace Duty.API.Duties.DeleteDuty;

public record DeleteDutyResponse(bool IsSuccess);

public class DeleteDutyEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/duties/{id}", async (int id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteDutyCommand(id));

            var response = result.Adapt<DeleteDutyResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteDuty")
        .Produces<DeleteDutyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .WithSummary("Delete Duty")
        .WithDescription("Delete Duty")
        .RequireAuthorization(policy => policy.RequireRole("Admin"));
    }
}
