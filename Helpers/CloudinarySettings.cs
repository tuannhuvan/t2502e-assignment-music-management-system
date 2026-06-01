namespace Music_Management_System.Helpers;

// Để hệ thống .NET có thể đọc hiểu và biến các chuỗi văn bản trong file JSON thành một đối tượng C# có các thuộc tính rõ ràng, chúng ta tạo một class trung gian CloudinarySettings
// Ánh xạ tự động (Configuration Binding): Khi ứng dụng chạy, .NET sẽ tự động đối chiếu các từ khóa trong JSON (CloudName, ApiKey...) với các thuộc tính cùng tên trong class C#.
// Việc này giúp gọi cấu hình bằng code dạng settings.CloudName thay vì phải gõ chuỗi thô, hạn chế tối đa sai chính tả.
public class CloudinarySettings
{
    public string CloudName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
}