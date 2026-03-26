using BuildingBlocks.Contracts.EmployeeContracts;

namespace Duty.API.Employees.GetEmployeeByUserId;

public record GetEmployeeByUserIdQuery(int UserId) : IQuery<GetEmployeeByUserIdResult>;
public record GetEmployeeByUserIdResult(EmployeeDto Employee);

internal class GetEmployeeByUserIdQueryHandler(IDocumentSession session)
    : IQueryHandler<GetEmployeeByUserIdQuery, GetEmployeeByUserIdResult>
{
    public async Task<GetEmployeeByUserIdResult> Handle(GetEmployeeByUserIdQuery query, CancellationToken cancellationToken)
    {
        var employee = await session.Query<Employee>()
            .Where(e => e.UserId == query.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (employee is null)
        {
            throw new Exception($"Employee with UserID {query.UserId} not found.");
        }

        var employeeDto = new EmployeeDto(employee.Id, employee.Name);
        return new GetEmployeeByUserIdResult(employeeDto);
    }
}
