namespace Duty.API.Clients.DeleteClient;

internal class DeleteClientCommandHandler(IDocumentSession session)
    : ICommandHandler<DeleteClientCommand, DeleteClientResult>
{
    public async Task<DeleteClientResult> Handle(DeleteClientCommand command, CancellationToken cancellationToken)
    {
        session.Delete<Client>(command.Id);

        await session.SaveChangesAsync(cancellationToken);

        return new DeleteClientResult(true);
    }
}