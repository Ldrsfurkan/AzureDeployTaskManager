namespace BuildingBlocks.Contracts.AuditContracts;

public record GetAuditLogsResponse(IEnumerable<AuditLogDto> Logs);