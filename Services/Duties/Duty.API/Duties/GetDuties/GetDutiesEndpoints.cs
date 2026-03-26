using BuildingBlocks.Contracts.DutyContracts;

namespace Duty.API.Duties.GetDuties;

public class GetDutiesEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/duties", async ([AsParameters] GetDutiesRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetDutiesQuery>();

            var result = await sender.Send(query);

            var response = result.Adapt<GetDutiesResponse>();

            return Results.Ok(response);
        })
        .WithName("GetDuties")
        .Produces<GetDutiesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Duties")
        .WithDescription("Get Duties");
    }
}