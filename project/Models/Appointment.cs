using System.ComponentModel.DataAnnotations;

namespace project.Models;

public class Appointment
{
    public int id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public string Email { get; set; } = "";

    [Required]
    public string Type { get; set; } = "";

    [Required]
    public string Date { get; set; } = "";

    public DateTime created_at { get; set; } = DateTime.UtcNow;
    
}