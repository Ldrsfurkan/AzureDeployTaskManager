using BuildingBlocks.Contracts.ClientContracts;

public record GetClientsResponse(IEnumerable<ClientDto> Clients);
