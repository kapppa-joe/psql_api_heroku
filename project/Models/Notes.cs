namespace project.Models;

public class Note
{
    public int id { get; set; }

    public string Title { get; set; } = "";

    public string Content { get; set; } = "";

    public DateTime created_at { get; set; }
    
}