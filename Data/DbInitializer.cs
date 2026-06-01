using Microsoft.EntityFrameworkCore;
using Music_Management_System.Models;

namespace Music_Management_System.Data
{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            // Reset dữ liệu cũ khi ở môi trường Dev (Xóa đi tạo lại DB)
            // context.Database.EnsureDeleted();
            // context.Database.EnsureCreated();

            // tạo một phạm vi (scope) tạm thời nhằm lấy ra các dịch vụ đã được đăng ký (Dependency Injection),
            // đặc biệt là từ class Program.cs khi ứng dụng đang khởi động (trước khi HTTP request đầu tiên được xử lý).
            // Khối using đảm bảo rằng sau khi thực hiện xong các đoạn code bên trong, toàn bộ các dịch vụ được tạo ra trong scope này sẽ tự động bị hủy (giải phóng bộ nhớ).
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var
                    services = serviceScope
                        .ServiceProvider; // Lấy ra bộ cung cấp dịch vụ (Service Provider) từ scope vừa tạo. Đây là công cụ giúp lấy các instance (thể hiện) của các class đã được đăng ký trong hệ thống DI
                var context =
                    services
                        .GetRequiredService<
                            AppDbContext>(); // Lấy instance của AppDbContext. Phương thức GetRequiredService sẽ báo lỗi (throw exception) nếu dịch vụ này chưa được đăng ký trong ứng dụng
                var logger =
                    services
                        .GetRequiredService<
                            ILogger<AppDbContext>>(); // Lấy instance của ILogger để có thể ghi log trong quá trình chạy đoạn code này (ví dụ: log lại lỗi nếu việc khởi tạo database thất bại).

                try
                {
                    logger.LogInformation("Bắt đầu quy trình khởi tạo Seeding database");
                    logger.LogInformation("Đang chạy kiểm tra và thực thi Migrations...");
                    context.Database.Migrate();
                    logger.LogInformation("Hệ thống Migrations đã đồng bộ thành công.");

                    // Khởi tạo Transaction để đảm bảo an toàn dữ liệu
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        logger.LogWarning("Đang xóa sạch dữ liệu cũ (Reset dữ liệu dev)...");
                        ClearData(context, logger);

                        logger.LogInformation("Đang chèn dữ liệu mẫu mới (Seeding)...");
                        SeedData(context, logger);

                        transaction.Commit();
                        logger.LogInformation("HOÀN TẤT SEEDING DỮ LIỆU THÀNH CÔNG!");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogInformation("[DB SEED ERROR] QUY TRÌNH THẤT BẠI. ĐÃ ROLLBACK DỮ LIỆU!");
                }
            }
        }

        private static void ClearData(AppDbContext context, ILogger<AppDbContext> logger)
        {
            // Sử dụng ExecuteDelete để xóa trực tiếp dưới DB, không tải lên RAM
            // Quy tắc: Xóa bảng con (Songs) chứa khóa ngoại trước, bảng cha (Singers, Composers) sau
            var deletedSongs = context.Songs.ExecuteDelete();
            logger.LogDebug($"- Đã xóa {deletedSongs} bài hát mẫu cũ.");

            var deletedSingers = context.Singers.ExecuteDelete();
            logger.LogDebug($"- Đã xóa {deletedSingers} ca sĩ mẫu cũ.");

            var deletedComposers = context.Composers.ExecuteDelete();
            logger.LogDebug($"- Đã xóa {deletedComposers} nhạc sĩ mẫu cũ.");
        
            logger.LogInformation("Đã dọn dẹp sạch sẽ dữ liệu cũ.");

        }

        private static void SeedData(AppDbContext context, ILogger<AppDbContext> logger)
        {
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
            logger.LogInformation($"Dang tao {singers.Count} ca sĩ mẫu...");
            
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
            logger.LogInformation($"Đang tạo {composers.Count} nhạc sĩ mẫu...");
            
            // Lưu lại để sinh Id tự động trước khi gán cho bài hát
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
            logger.LogInformation($"Đang tạo vòng lặp sinh {songs.Count} bài hát liên kết ngẫu nhiên...");
            context.SaveChanges();
        }


        // Kiểm tra nếu đã có dữ liệu thì không seed nữa để tránh trùng lặp
            // if (context.Singers.Any() || context.Composers.Any()) return;





            
            // Lưu lại để sinh Id cho Singer và Composer trước khi gán cho Song
            // context.SaveChanges(); 


    }
}
