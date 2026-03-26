using Marten.Pagination;

namespace Duty.API.Duties.GetDuties
{
    public record GetDutiesQuery(int? PageNumber = 1, int? PageSize = 20) : IQuery<GetDutiesResult>;
    public record GetDutiesResult(IEnumerable<DutyDto> Duties);

    internal class GetDutiesQueryHandler(IDocumentSession session)
        : IQueryHandler<GetDutiesQuery, GetDutiesResult>
    {
        public async Task<GetDutiesResult> Handle(GetDutiesQuery query, CancellationToken cancellationToken)
        {
            var employeeDict = new Dictionary<int, Employee>();
            var clientDict = new Dictionary<int, Client>();

            // Fetch duties and tell Marten to fill the dictionaries with related data
            var duties = await session.Query<DutyEntity>()
                .Include(d => d.AssignedEmployeeId, employeeDict)
                .Include(d => d.ClientId, clientDict)
                .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 20, cancellationToken);

            // Map the Duty entities to DutyDto safely
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

                // Look up the name from the dictionary, fallback to a default text if null
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
}
