namespace Duty.API.Models;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Action { get; set; } = string.Empty; // e.g., "Task Updated", "Client Deleted"
    public string Details { get; set; } = string.Empty; // e.g., "Task 'Fix Login' was marked as Completed."
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
