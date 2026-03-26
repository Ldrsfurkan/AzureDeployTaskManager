namespace BuildingBlocks.Events;

public record UserRegisteredIntegrationEvent(int UserId, string Username);