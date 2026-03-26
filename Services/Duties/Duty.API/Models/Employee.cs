using System.ComponentModel.DataAnnotations;

namespace Duty.API.Models;  

public class Employee
{
    public int Id { get; set; }
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
    //Auth.Api UserId
    public int UserId { get; set; }
    public string UserRole { get; set; }
}