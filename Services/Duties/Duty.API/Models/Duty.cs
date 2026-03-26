using System.ComponentModel.DataAnnotations;

namespace Duty.API.Models
{
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
    public class Duty
    {
        [Key]
        public int Id { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }
        [StringLength(200)]
        public string? MailDescription { get; set; }
        public int Priority { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        //public Employee Employee { get; set; }
        //public Client Client { get; set; }
        public int? AssignedEmployeeId { get; set; }
        public int? ClientId { get; set; }
    }
}
