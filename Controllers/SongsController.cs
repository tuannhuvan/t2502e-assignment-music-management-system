using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music_Management_System.Data;
using Music_Management_System.Interfaces;
using Music_Management_System.Models;
using Music_Management_System.ViewModels;

namespace Music_Management_System.Controllers
{
    public class SongsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;

        // Cơ chế Dependency Injection (DI): Hệ thống tự động tiêm DbContext và PhotoService vào đây
        public SongsController(AppDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }
        
        // DANH SÁCH BÀI HÁT (INDEX) - TÌM KIẾM & PHÂN TRANG

        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 10; // yêu cầu: 10 bài hát/trang

            // Khởi tạo câu lệnh LINQ cơ bản: Chỉ lấy các bài hát chưa bị xóa mềm (Status == 1)
            // Sử dụng .Include() để nạp Eager Loading dữ liệu từ bảng Singer và Composer (Tránh lỗi N+1 Query)
            var query = _context.Songs
                .Include(s => s.Singer)
                .Include(s => s.Composer)
                .Where(s => s.Status == 1)
                .AsQueryable();

            // Lọc dữ liệu theo tiêu đề (Title) nếu người dùng có nhập từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Title.Contains(searchString));
            }

            // Tính toán tổng số lượng bản ghi sau khi lọc để phục vụ phân trang
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Thực thi phân trang dưới MySQL bằng cú pháp Skip (Bỏ qua) và Take (Lấy)
            var songs = await query
                .OrderByDescending(s => s.CreatedAt) // Bài mới tạo lên đầu
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Đẩy các thông số điều hướng ra ngoài giao diện thông qua ViewData
            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;

            return View(songs);
        }
        
        // THÊM MỚI BÀI HÁT - GIAO DIỆN (GET)
        [HttpGet]
        public IActionResult Create()
        {
            // Nạp danh sách Ca sĩ và Nhạc sĩ vào ViewBag dưới dạng SelectList để sinh Dropdown Menu (Select) ở View
            ViewBag.SingerId = new SelectList(_context.Singers, "Id", "Name");
            ViewBag.ComposerId = new SelectList(_context.Composers, "Id", "Name");
            return View();
        }
        
        // THÊM MỚI BÀI HÁT - XỬ LÝ LƯU (POST)
        [HttpPost]
        [ValidateAntiForgeryToken] // Ngăn chặn tấn công giả mạo yêu cầu chéo trang CSRF
        public async Task<IActionResult> Create(SongCreateViewModel model)
        {
            // Validation Nghiệp vụ: Ngày phát hành không được lớn hơn ngày hiện tại
            if (model.ReleaseDate > DateTime.Now)
            {
                ModelState.AddModelError("ReleaseDate", "Ngày phát hành không được lớn hơn ngày hiện tại!");
            }

            // Nếu dữ liệu hợp lệ (vượt qua tất cả các bộ lọc Validation ở ViewModel và Nghiệp vụ)
            if (ModelState.IsValid)
            {
                string imageUrl = "https://res.cloudinary.com/demo/image/upload/v1570975253/sample.jpg"; // Ảnh mặc định

                // Nếu người dùng có chọn file ảnh vật lý từ máy tính
                if (model.ImageFile != null)
                {
                    // Gọi PhotoService đẩy ảnh lên Cloudinary
                    var uploadResult = await _photoService.AddPhotoAsync(model.ImageFile);
                    
                    if (uploadResult.Error != null)
                    {
                        ModelState.AddModelError("ImageFile", "Tải ảnh lên Cloudinary thất bại. Vui lòng thử lại!");
                        ViewBag.SingerId = new SelectList(_context.Singers, "Id", "Name", model.SingerId);
                        ViewBag.ComposerId = new SelectList(_context.Composers, "Id", "Name", model.ComposerId);
                        return View(model);
                    }
                    
                    // Lấy link URL an toàn do Cloudinary trả về
                    imageUrl = uploadResult.SecureUrl.AbsoluteUri;
                }

                // Ánh xạ (Map) dữ liệu từ chiếc hộp an toàn ViewModel sang Entity Model thực tế
                var song = new Song
                {
                    Title = model.Title,
                    Lyrics = model.Lyrics,
                    ThumbnailUrl = imageUrl,
                    Mp3Link = model.Mp3Link,
                    ReleaseDate = model.ReleaseDate,
                    SingerId = model.SingerId,
                    ComposerId = model.ComposerId,
                    Status = 1, // Mặc định là Active
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Lưu vào CSDL MySQL
                _context.Songs.Add(song);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            // Nếu dữ liệu lỗi, nạp lại Dropdown và trả lại Form kèm thông báo lỗi chi tiết
            ViewBag.SingerId = new SelectList(_context.Singers, "Id", "Name", model.SingerId);
            ViewBag.ComposerId = new SelectList(_context.Composers, "Id", "Name", model.ComposerId);
            return View(model);
        }
        
        // CHI TIẾT BÀI HÁT (DETAILS)
        public async Task<IActionResult> Details(int id)
        {
            var song = await _context.Songs
                .Include(s => s.Singer)
                .Include(s => s.Composer)
                .FirstOrDefaultAsync(s => s.Id == id && s.Status == 1);

            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }
        
        // XÓA MỀM BÀI HÁT (DÙNG AJAX/FETCH POST)
        [HttpPost]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return Json(new { success = false, message = "Không tìm thấy bài hát cần xóa!" });
            }

            // yêu cầu: Không xóa bản ghi khỏi DB mà chuyển Status về 0
            song.Status = 0;
            song.UpdatedAt = DateTime.Now;

            _context.Songs.Update(song);
            await _context.SaveChangesAsync();

            // Trả về kết quả dạng JSON để JavaScript (Phía Client) xử lý mượt mà không bị reload trang
            return Json(new { success = true, message = "Đã ẩn bài hát thành công (Xóa mềm)!" });
        }
    }
}