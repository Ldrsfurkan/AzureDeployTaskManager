namespace Duty.API.Clients.AddClients;

public record AddClientCommand(string Name) : ICommand<AddClientResult>;
public record AddClientResult(int Id);

internal class AddClientCommandHandler(IDocumentSession session)
    : ICommandHandler<AddClientCommand, AddClientResult>
{
    public async Task<AddClientResult> Handle(AddClientCommand command, CancellationToken cancellationToken)
    {
        var client = new Client
        {
            Name = command.Name,
        };

        session.Store(client);
        await session.SaveChangesAsync(cancellationToken);

        return new AddClientResult(client.Id);
    }
}
