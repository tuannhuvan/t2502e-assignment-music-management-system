using System.ComponentModel.DataAnnotations;

namespace Music_Management_System.Models;

public class Singer
{
    [Key] // Dinh nghia khoa chinh
    public int Id { get; set; }
    
    [Required] // bat buoc khong de trong trong db
    [MaxLength(255)]
    public string Name { get; set; }
    public string? Biography { get; set; }
    public string? ImageUrl { get; set; }
    
    // Navigation Property: Quan he 1 ca si co nhieu bai hat
    public ICollection<Song> Songs { get; set; } = new List<Song>();
}