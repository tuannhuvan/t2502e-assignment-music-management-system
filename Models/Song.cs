using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Music_Management_System.Models;

public class Song
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }
    public string? Lyrics { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Mp3Link { get; set; }
    public DateTime ReleaseDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    // Khoa ngoai lien ket toi bang Singer
    public int SingerId { get; set; }
    [ForeignKey("SingerId")] // Khai bao tuong minh
    public Singer Singer { get; set; } // Doi tuong Singer tuong ung
    
    // Khoa ngoai lien ket toi bang Composer
    public int ComposerId { get; set; }
    [ForeignKey("ComposerId")]
    public Composer Composer { get; set; }
    
    // Trang thai 1 = Active, 0 = Soft Deleted
    public int Status { get; set; } = 1;
}