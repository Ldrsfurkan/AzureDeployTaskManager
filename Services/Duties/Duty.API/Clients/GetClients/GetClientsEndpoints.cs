namespace Duty.API.Clients.GetClients;

public record GetClientsResponse(IEnumerable<Client> Clients);
public class GetClientsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/clients", async (ISender sender) =>
        {
            var result = await sender.Send(new GetClientsQuery());

            var response = result.Adapt<GetClientsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetClients")
        .Produces<GetClientsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get All Clients")
        .WithDescription("Get All Clients for Dropdowns");
    }
}
