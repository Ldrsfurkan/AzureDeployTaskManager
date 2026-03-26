using Duty.API.Duties.GetDuties;

namespace Duty.API.Duties.GetDutiesByEmployee;

public record GetDutiesByEmployeeQuery(int EmployeeId) : IQuery<GetDutiesResult>;

internal class GetDutiesByEmployeeQueryHandler(IDocumentSession session)
    : IQueryHandler<GetDutiesByEmployeeQuery, GetDutiesResult>
{
    public async Task<GetDutiesResult> Handle(GetDutiesByEmployeeQuery query, CancellationToken cancellationToken)
    {
        var employeeDict = new Dictionary<int, Employee>();
        var clientDict = new Dictionary<int, Client>();

        // Notice the .Where() filter applied for the specific EmployeeId
        var duties = await session.Query<DutyEntity>()
            .Where(duty => duty.AssignedEmployeeId == query.EmployeeId)
            .Include(duty => duty.AssignedEmployeeId.Value, employeeDict)
            .Include(duty => duty.ClientId.Value, clientDict)
            .ToListAsync(cancellationToken);

        var dutyDtos = duties.Select(duty => new DutyDto
        {
            Id = duty.Id,
            Description = duty.Description,
            MailDescription = duty.MailDescription,
            Priority = duty.Priority,
            Status = duty.Status,
            CreatedAt = duty.CreatedAt,
            UpdatedAt = duty.UpdatedAt,
            AssignedEmployeeId = duty.AssignedEmployeeId,
            ClientId = duty.ClientId,

            AssignedEmployeeName = duty.AssignedEmployeeId.HasValue && employeeDict.TryGetValue(duty.AssignedEmployeeId.Value, out var employee)
                ? employee.Name
                : "Unassigned",

            ClientName = duty.ClientId.HasValue && clientDict.TryGetValue(duty.ClientId.Value, out var client)
                ? client.Name
                : "No Client"
        }).ToList();

        return new GetDutiesResult(dutyDtos);
    }
}
