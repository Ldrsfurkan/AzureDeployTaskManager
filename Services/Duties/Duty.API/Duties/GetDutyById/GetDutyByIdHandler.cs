
namespace Duty.API.Duties.GetDutyById
{
    public record GetDutyByIdQuery(int Id) : IQuery<GetDutyByIdResult>;
    public record GetDutyByIdResult(DutyEntity Duty);

    internal class GetDutyByIdQueryHandler(IDocumentSession session)
        : IQueryHandler<GetDutyByIdQuery, GetDutyByIdResult>
    {
        public async Task<GetDutyByIdResult> Handle(GetDutyByIdQuery query, CancellationToken cancellationToken)
        {
            var duty = await session.LoadAsync<DutyEntity>(query.Id, cancellationToken);

            if (duty is null)
            {
                throw new DutyNotFoundException(query.Id);
            }

            return new GetDutyByIdResult(duty);
        }
    }
}
