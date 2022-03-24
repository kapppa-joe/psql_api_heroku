using System.ComponentModel.DataAnnotations;

namespace project.Models;

public class Note
{
    public int id { get; set; }

    [Required]
    public string Title { get; set; } = "";

    [Required]
    public string Content { get; set; } = "";

    public DateTime created_at { get; set; } = DateTime.UtcNow;
    
}