using BuildingBlocks.CQRS;

namespace BuildingBlocks.Contracts.ClientContracts;

public record DeleteClientCommand(int Id) : ICommand<DeleteClientResult>;
public record DeleteClientResult(bool IsSuccess);