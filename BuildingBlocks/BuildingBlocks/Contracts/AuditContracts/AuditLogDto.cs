namespace BuildingBlocks.Contracts.AuditContracts;

public record AuditLogDto(string Action, string Details, DateTime Timestamp);