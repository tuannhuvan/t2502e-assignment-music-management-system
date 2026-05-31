using Microsoft.EntityFrameworkCore;
using Music_Management_System.Models;

namespace Music_Management_System.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Singer> Singers { get; set; }
    public DbSet<Composer> Composers { get; set; }
    public DbSet<Song> Songs { get; set; }
    
    // Use Fluent API 
}