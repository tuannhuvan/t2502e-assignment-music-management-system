# 🎵 Music Management System (.NET 8.0 MVC & MySQL)
Một ứng dụng quản lý âm nhạc hoàn chỉnh được xây dựng trên nền tảng **ASP.NET Core 8.0 MVC**, sử dụng **Entity Framework Core (Code-First)** kết nối cơ sở dữ liệu **MySQL**. [cite_start]Dự án tích hợp các công nghệ hiện đại bao gồm dịch vụ lưu trữ đám mây **Cloudinary** để quản lý hình ảnh, bộ soạn thảo văn bản giàu **CKEditor 5**, phân trang, tìm kiếm nâng cao và xử lý xóa mềm bằng **AJAX Fetch API**.

---

## 🚀 Tính Năng Chính Dự Án

* **Quản lý bài hát (CRUD Thủ công):** Được triển khai 100% bằng tay (No Scaffolding) giúp kiểm soát tối đa cấu trúc mã nguồn.
* **Data Seeding tự động:** Tự động dọn dẹp dữ liệu cũ (Reset) và nạp 05 Ca sĩ, 05 Nhạc sĩ cùng 50 bài hát mẫu ngay khi khởi chạy ứng dụng ở môi trường Development.
* **Tìm kiếm & Phân trang:** Hỗ trợ tìm kiếm tối ưu theo tiêu đề bài hát và cắt nhỏ dữ liệu hiển thị (10 bài hát/trang) giúp giảm tải hệ thống.
* **Trình phát nhạc toàn cục (Global Player):** Cho phép người dùng nghe nhạc trực tiếp tại trang danh sách hoặc trang chi tiết thông qua luồng phát âm thanh `.mp3` trực tuyến.
* **Tích hợp Đám mây Cloudinary:** Tự động xử lý kích cỡ, nén ảnh vuông 500x500 (Crop Fill) và lưu trữ ảnh bìa trực tuyến thông qua CDN bảo mật.
* **Văn bản giàu (Rich Text):** Sử dụng CKEditor 5 để biên tập lời bài hát và render chuẩn xác bằng cấu trúc `@Html.Raw()`.
* **Xóa mềm (Soft Delete) bằng AJAX:** Sử dụng Fetch API để chuyển trạng thái bài hát về `0` (Ẩn khỏi giao diện) mà không làm tải lại trang (No Reload).

---

## 🛠️ Công Nghệ Sử Dụng

* **Framework chính:** .NET 8.0 (ASP.NET Core MVC) 
* **ORM (Cầu nối DB):** Entity Framework Core (Pomelo MySQL Provider) 
* **Database:** MySQL Server 
* **Thư viện bên thứ ba:**
    * `CloudinaryDotNet` (Hỗ trợ API Upload ảnh) 
    * `CKEditor 5 CDN` (Bộ soạn thảo văn bản giàu) 
    * `Bootstrap 5` (Thiết kế giao diện Responsive) 

---

## 📂 Kiến Trúc & Luồng Thao Tác Dữ Liệu

Dự án tuân thủ nghiêm ngặt nguyên lý thiết kế hướng đối tượng và bảo mật dữ liệu thông qua kiến trúc **ViewModel**:

* **Entities Model (`Models/`):** Đại diện thuần túy cho cấu trúc các bảng dưới MySQL (`Singer`, `Composer`, `Song`).
* **ViewModels (`ViewModels/`):** Đóng vai trò là chiếc "hộp bảo mật" trung gian, chứa các điều kiện Validation đầu vào (`[Required]`, `[MinLength]`) và thuộc tính `IFormFile` để hứng file ảnh vật lý từ Form. Ngăn chặn hoàn toàn lỗ hổng bảo mật **Over-Posting**.

---

## ⚙️ Hướng Dẫn Cài Đặt & Chạy Dự Án

### 1. Chuẩn bị môi trường
* Cài đặt [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
* Chạy hệ quản trị CSDL MySQL (XAMPP, Laragon, MySQL Workbench...).

### 2. Cấu hình thông số ứng dụng
Mở file `appsettings.json` tại thư mục gốc và thay đổi thông số kết nối MySQL cùng tài khoản Cloudinary của bạn:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=music_management_system;user=root;password=Mật_Khẩu_MySQL_Của_Bạn"
  },
  "CloudinarySettings": {
    "CloudName": "tên_cloud_của_bạn",
    "ApiKey": "api_key_của_bạn",
    "ApiSecret": "api_secret_của_bạn"
  }
}
### 3. Thực thi Khởi tạo Cơ sở dữ liệu
Mở terminal tại thư mục gốc dự án và chạy chuỗi lệnh sau để EF Core quét cấu hình Code-First và sinh bảng vào MySQL:
# Tạo bản thiết kế Migration ban đầu
dotnet ef migrations add InitialCreate
Đẩy bản thiết kế thành các bảng thực tế dưới MySQL
dotnet ef database update

### 4. Khởi chạy ứng dụng
Thực thi lệnh sau để kích hoạt tính năng Hot Reload, ứng dụng sẽ tự động chạy và mở trình duyệt:  Bashdotnet watch run
Sau khi trình duyệt xuất hiện, bạn truy cập đường dẫn /Songs (Ví dụ: https://localhost:7001/Songs) để bắt đầu trải nghiệm hệ thống.
📝 Quy Tắc Phát Triển Code Cần Lưu Ý
1. Thiếu Enctype trên Form: Mọi form có tải tệp tin (ImageFile) bắt buộc phải bọc thuộc tính enctype="multipart/form-data" ở thẻ <form>, nếu không dữ liệu file gửi lên Controller sẽ bị null.
2. Xử lý Bất đồng bộ (Async/Await): Các tác vụ tương tác với MySQL (SaveChangesAsync) hoặc gọi API Cloudinary (UploadAsync) đều là tác vụ I/O, bắt buộc phải sử dụng cơ chế async/await để tránh nghẽn luồng hệ thống.
3. Render HTML an toàn: Sử dụng @Html.Raw() một cách có kiểm soát tại trang chi tiết để trình duyệt biên dịch mã từ CKEditor, nhưng luôn tận dụng ModelState.IsValid để lọc dữ liệu bẩn từ phía người dùng.  
