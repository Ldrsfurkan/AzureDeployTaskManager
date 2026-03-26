namespace BuildingBlocks.Contracts.EmployeeContracts;

public record CreateEmployeeRequest(int UserId, string Name, string UserRole);