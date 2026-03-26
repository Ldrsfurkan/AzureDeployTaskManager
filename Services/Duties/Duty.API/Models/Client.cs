using System.ComponentModel.DataAnnotations;

namespace Duty.API.Models
{
    public class Client
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
    }
}
