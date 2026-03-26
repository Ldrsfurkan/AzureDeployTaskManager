namespace BuildingBlocks.Contracts.DutyContracts;

public class UpdateDutyRequest
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? MailDescription { get; set; }
    public int Priority { get; set; }
    public string? Status { get; set; }
    public int? AssignedEmployeeId { get; set; }
    public int? ClientId { get; set; }
}
