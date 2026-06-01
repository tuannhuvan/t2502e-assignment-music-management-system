using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    
    // [NotMapped]
    // [Display(Name = "Tải ảnh đại diện")]
    // public IFormFile? ImageFile { get; set; } // Nhận file từ ViewForm
    
    // Navigation Property: Quan he 1 ca si co nhieu bai hat
    public ICollection<Song> Songs { get; set; } = new List<Song>();
}