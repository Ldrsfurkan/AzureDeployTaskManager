namespace Duty.API.Duties.UpdateDuty;


public class UpdateDutyEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/duties",
            async (UpdateDutyRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateDutyCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateDutyResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateDuty")
            .Produces<UpdateDutyResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Duty")
            .WithDescription("Update Duty");
    }
}
