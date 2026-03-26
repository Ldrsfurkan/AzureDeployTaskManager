using BuildingBlocks.Contracts.EmployeeContracts;
namespace Duty.API.Employees.CreateEmployee;
public record CreateEmployeeResponse(int Id);

public class CreateEmployeeEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/employees", async (CreateEmployeeRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateEmployeeCommand>();

            var result = await sender.Send(command);
            var response = result.Adapt<CreateEmployeeResponse>();

            return Results.Created($"/employees/{response.Id}", response);
        })
        .WithName("CreateEmployee")
        .Produces<CreateEmployeeResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}