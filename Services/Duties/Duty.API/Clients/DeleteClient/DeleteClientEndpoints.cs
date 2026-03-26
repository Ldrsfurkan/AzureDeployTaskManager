namespace Duty.API.Clients.DeleteClient;
public class DeleteClientEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/clients/{id}", async (int id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteClientCommand(id));

            return Results.Ok(result);
        })
        .WithName("DeleteClient")
        .Produces<DeleteClientResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Client")
        .WithDescription("Delete a client by Id");
    }
}
