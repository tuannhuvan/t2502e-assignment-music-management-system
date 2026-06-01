using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Music_Management_System.Helpers;
using Music_Management_System.Interfaces;

namespace Music_Management_System.Services
{
    public class PhotoService : IPhotoService
    {
    private readonly Cloudinary _cloudinary;

    // Constructor nhận cấu hình thông qua IOptions mẫu thiết kế chuẩn của .NET
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        // Khởi tạo một tài khoản Cloudinary từ các thông số được nạp từ appsettings.json
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        // Tạo đối tượng trung tâm để gọi API lên Cloudinary server
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        // Kiểm tra xem file người dùng tải lên có rỗng hay không
        if (file != null && file.Length > 0)
        {
            // Mở một luồng đọc dữ liệu (Stream) dạng nhị phân từ file
            using var stream = file.OpenReadStream();

            // Thiết lập các thông số tải lên
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                // Tự động biến đổi kích thước ảnh về dạng vuông 500x500 và cắt đi phần thừa (fill)
                Transformation = new Transformation().Height(500).Width(500).Crop("fill")
            };

            // Gửi file bất đồng bộ lên máy chủ Cloudinary và nhận kết quả trả về
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        return await _cloudinary.DestroyAsync(deleteParams);
    }
    }
}