namespace Duty.API.Employees.GetEmployees
{
    public record GetEmployeesResponse(IEnumerable<Employee> Employees);

    public class GetEmployeesEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/employees", async (ISender sender) =>
            {
                var result = await sender.Send(new GetEmployeesQuery());

                var response = result.Adapt<GetEmployeesResponse>();

                return Results.Ok(response);
            })
            .WithName("GetEmployees")
            .Produces<GetEmployeesResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get All Employees")
            .WithDescription("Get All Employees for Dropdowns");
        }
    }
}
