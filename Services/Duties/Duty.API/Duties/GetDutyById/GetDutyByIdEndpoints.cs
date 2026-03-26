namespace Duty.API.Duties.GetDutyById;

public record GetDutyByIdResponse(DutyEntity Duty);

public class GetDutyByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/duties/{id}", async (int id, ISender sender) =>
        {
            var result = await sender.Send(new GetDutyByIdQuery(id));

            var response = result.Adapt<GetDutyByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetDutyById")
        .Produces<GetDutyByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Duty By Id")
        .WithDescription("Get Duty By Id");
    }
}