using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

// Theo yêu cầu của đề bài, chúng ta cần một biểu mẫu (Form) để người dùng nhập thông tin Bài hát.
// Tuy nhiên, class Song.cs gốc chỉ có thuộc tính string? ThumbnailUrl (dùng để lưu link chuỗi vào CSDL), hoàn toàn không có thuộc tính nào để nhận diện file ảnh tải lên từ máy tính.
// Vì vậy, chúng ta tạo ra một lớp ViewModel chuyên biệt để phục vụ riêng cho việc truyền/nhận dữ liệu ở giao diện Form.
// 1. Giải pháp của ViewModel: ViewModel sinh ra chỉ chứa đúng các trường cho phép nhập (Ví dụ: Title, Lyrics, ImageFile).
// Kẻ xấu có cố tình nhét thêm trường Status vào Form thì Model Binder cũng không tìm thấy thuộc tính đó trong ViewModel để gán $\rightarrow$ An toàn tuyệt đối
// 2. Giải pháp của ViewModel: Sẽ tạo ra SongCreateViewModel (có thuộc tính ảnh là [Required]) và SongEditViewModel (thuộc tính ảnh có thể null). Cả hai sau khi xử lý xong đều map về chung một Entity Song để lưu xuống DB.
// 3. Giải quyết được Ô nhiễm dữ liệu (Polluted Domain Model)
namespace Music_Management_System.ViewModels
{
    public class SongCreateViewModel
    {
        [Required(ErrorMessage = "Tiêu đề bài hát bắt buộc phải nhập!")]
        [MinLength(5, ErrorMessage = "Tiêu đề bài hát phải có tối thiểu 5 ký tự!")]
        public string Title { get; set; } = string.Empty;

        public string? Lyrics { get; set; }

        // Thẻ này đại diện cho file ảnh vật lý người dùng chọn từ máy tính
        [Display(Name = "Ảnh bìa bài hát")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Đường dẫn file nhạc MP3 không được để trống!")]
        public string Mp3Link { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn ngày phát hành!")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Vui lòng chọn một Ca sĩ!")]
        public int SingerId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn một Nhạc sĩ!")]
        public int ComposerId { get; set; }
    }
}