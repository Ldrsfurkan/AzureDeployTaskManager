namespace Duty.API.Clients.AddClients;

public class AddClientEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/clients", async (AddClientRequest request, ISender sender) =>
        {
            var command = request.Adapt<AddClientCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<AddClientResponse>();

            return Results.Created($"/clients/{response.Id}", response);
        })
        .WithName("AddClient")
        .Produces<AddClientResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add Client")
        .WithDescription("Create a new client");
    }
}
