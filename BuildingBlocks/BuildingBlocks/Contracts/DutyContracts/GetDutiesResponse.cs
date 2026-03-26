namespace BuildingBlocks.Contracts.DutyContracts;

public class GetDutiesResponse
{
    public IEnumerable<DutyDto> Duties { get; set; } = Enumerable.Empty<DutyDto>();
}
