namespace Duty.API.Clients.GetClients
{
    public record GetClientsQuery() : IQuery<GetClientsResult>;
    public record GetClientsResult(IEnumerable<Client> Clients);

    internal class GetClientsQueryHandler(IDocumentSession session)
        : IQueryHandler<GetClientsQuery, GetClientsResult>
    {
        public async Task<GetClientsResult> Handle(GetClientsQuery query, CancellationToken cancellationToken)
        {
            var clients = await session.Query<Client>()
                .ToListAsync(cancellationToken);

            return new GetClientsResult(clients);
        }
    }
}
