namespace Duty.API.Duties.GetDutiesByClient;

public record GetDutyByClientQuery(int ClientId) : IQuery<GetDutiesByClientResult>;
public record GetDutiesByClientResult(IEnumerable<DutyEntity> Duties);

internal class GetDutiesByClientQueryHandler(IDocumentSession session)
    : IQueryHandler<GetDutyByClientQuery, GetDutiesByClientResult>
{
    public async Task<GetDutiesByClientResult> Handle(GetDutyByClientQuery query, CancellationToken cancellationToken)
    {
        var duties = await session.Query<DutyEntity>()
            .Where(p => p.ClientId == query.ClientId)
            .ToListAsync(cancellationToken);

        return new GetDutiesByClientResult(duties);
    }
}
