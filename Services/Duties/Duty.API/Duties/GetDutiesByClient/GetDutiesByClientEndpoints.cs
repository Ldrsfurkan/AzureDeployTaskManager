namespace Duty.API.Duties.GetDutiesByClient;
public class GetDutiesByClientEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/duties/client/{clientId}",
            async (int clientId, ISender sender) =>
            {
                var result = await sender.Send(new GetDutyByClientQuery(clientId));

                var response = result.Adapt<GetDutiesByClientResponse>();

                return Results.Ok(response);
            })
        .WithName("GetDutiesByClientQuery")
        .Produces<GetDutiesByClientResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Duties By Client ")
        .WithDescription("Get Duties From a Client");
    }
}
