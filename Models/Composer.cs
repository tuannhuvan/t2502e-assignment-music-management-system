using System.ComponentModel.DataAnnotations;

namespace Music_Management_System.Models;

public class Composer
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    public string? Biography { get; set; }
    public string? ImageUrl { get; set; }
    
    // Navigation Property: Quan he 1 nhac si co th sang tac nhieu bai hat
    public ICollection<Song> Songs { get; set; } = new List<Song>();
}