namespace BuildingBlocks.Contracts.EmployeeContracts;

public record GetUsersResult(IEnumerable<UserDto> Users);