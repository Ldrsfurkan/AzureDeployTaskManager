namespace Shared.Events;
public record class UserRoleUpdatedEvent(int UserId, string NewRole);