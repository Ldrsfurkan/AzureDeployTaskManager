namespace BuildingBlocks.Contracts.DutyContracts;
public record GetDutiesByClientResponse(IEnumerable<DutyDto> Duties);