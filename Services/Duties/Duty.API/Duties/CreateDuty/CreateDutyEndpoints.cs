using BuildingBlocks.Contracts.DutyContracts;

namespace Duty.API.Duties.CreateDuty;
public class CreateDutyEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/duties",
            async (CreateDutyRequest request, ISender Sender) =>
            {
                var command = request.Adapt<CreateDutyCommand>();
                var result = await Sender.Send(command);
                var response = result.Adapt<CreateDutyResponse>();
                return Results.Created($"/duties/{response.Id}", response);
            })
            .WithName("CreateDuty")
            .Produces<CreateDutyResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Duty")
            .WithDescription("Create Duty");
    }
}
