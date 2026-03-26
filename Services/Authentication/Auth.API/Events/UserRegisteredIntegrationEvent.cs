namespace Shared.Events;

public record UserRegisteredIntegrationEvent(int UserId, string Username);