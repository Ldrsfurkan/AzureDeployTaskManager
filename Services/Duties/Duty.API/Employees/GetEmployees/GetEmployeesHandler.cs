namespace Duty.API.Employees.GetEmployees
{
    public record GetEmployeesQuery() : IQuery<GetEmployeesResult>;
    public record GetEmployeesResult(IEnumerable<Employee> Employees);

    internal class GetEmployeesQueryHandler(IDocumentSession session)
        : IQueryHandler<GetEmployeesQuery, GetEmployeesResult>
    {
        public async Task<GetEmployeesResult> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
        {
            // Fetching all employees without pagination for the dropdown list
            var employees = await session.Query<Employee>()
                .ToListAsync(cancellationToken);

            return new GetEmployeesResult(employees);
        }
    }
}
