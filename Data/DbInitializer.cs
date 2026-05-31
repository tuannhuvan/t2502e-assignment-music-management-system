using Music_Management_System.Models;

namespace Music_Management_System.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            // Reset dữ liệu cũ khi ở môi trường Dev (Xóa đi tạo lại DB)
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Kiểm tra nếu đã có dữ liệu thì không seed nữa để tránh trùng lặp
            if (context.Singers.Any() || context.Composers.Any()) return;

            // Khởi tạo 05 Ca sĩ mẫu
            var singers = new List<Singer>
            {
                new Singer { Name = "Sơn Tùng M-TP", Biography = "<p>Ca sĩ nổi tiếng tại Việt Nam.</p>", ImageUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg" },
                new Singer { Name = "Đen Vâu", Biography = "<p>Rapper mang phong cách mộc mạc.</p>", ImageUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg" },
                new Singer { Name = "Mỹ Tâm", Biography = "<p>Họa mi tóc nâu.</p>", ImageUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg" },
                new Singer { Name = "Soobin Hoàng Sơn", Biography = "<p>Hoàng tử Ballad và R&B.</p>", ImageUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg" },
                new Singer { Name = "Vũ", Biography = "<p>Hoàng tử indie Việt.</p>", ImageUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg" }
            };
            context.Singers.AddRange(singers);

            // Khởi tạo 05 Nhạc sĩ mẫu
            var composers = new List<Composer>
            {
                new Composer { Name = "Nguyễn Đức Cường", Biography = "<p>Tác giả bài nồng nàn Hà Nội.</p>", ImageUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg" },
                new Composer { Name = "Tiên Cookie", Biography = "<p>Nhạc sĩ tạo hit cho Soobin, Bích Phương.</p>", ImageUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg" },
                new Composer { Name = "Khắc Hưng", Biography = "<p>Nhà sản xuất âm nhạc hàng đầu.</p>", ImageUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg" },
                new Composer { Name = "Hứa Kim Tuyền", Biography = "<p>Nhạc sĩ trẻ tài năng với nhiều hit quốc dân.</p>", ImageUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg" },
                new Composer { Name = "Phan Mạnh Quỳnh", Biography = "<p>Nhạc sĩ mang âm hưởng tự sự sâu sắc.</p>", ImageUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg" }
            };
            context.Composers.AddRange(composers);
            
            // Lưu lại để sinh Id cho Singer và Composer trước khi gán cho Song
            context.SaveChanges(); 

            // Khởi tạo 50 bài hát mẫu bằng vòng lặp
            var songs = new List<Song>();
            // Chạy từ 0 đến 49 để phép chia dư i % 5 lấy chuẩn xác từ index 0 đến 4
            for (int i = 0; i < 50; i++)
            {
                songs.Add(new Song
                {
                    Title = $"Bài hát mẫu số {i + 1}",
                    Lyrics = $"<p>Đây là lời chi tiết cho bài hát mẫu số {i + 1}. Lời bài hát rất hay và ý nghĩa...</p>",
                    ThumbnailUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg",
                    Mp3Link = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3",
                    ReleaseDate = DateTime.Now.AddDays(-i),
                    SingerId = singers[i % 5].Id, // Lấy tuần tự từ index 0 đến 4
                    ComposerId = composers[i % 5].Id, 
                    Status = 1
                });
            }
            context.Songs.AddRange(songs);
            context.SaveChanges(); 
        }
    }
}
