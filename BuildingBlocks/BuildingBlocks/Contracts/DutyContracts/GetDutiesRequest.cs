namespace BuildingBlocks.Contracts.DutyContracts;

public class GetDutiesRequest
{
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 20;
}