using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Contracts.DutyContracts;

public enum DutyStatus
{
    BEKLİYOR,
    DEVAM_EDIYOR,
    YANIT_BEKLENIYOR,
    TEST_BEKLENIYOR,
    TAMAMLANDI,
    IPTAL_EDILDI,
    KULLANCI_ONAYI_BEKLENIYOR,
    EKSTRA_DURUM
}
public class DutyDto
{
    [Key]
    public int Id { get; set; }
    [StringLength(100)]
    public string? Description { get; set; }
    [StringLength(100)]
    public string? MailDescription { get; set; }
    public int Priority { get; set; }
    [StringLength(50)]
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? AssignedEmployeeId { get; set; }
    [StringLength(50)]
    public string? AssignedEmployeeName { get; set; }
    public int? ClientId { get; set; }
    [StringLength(50)]
    public string? ClientName { get; set; }
}
