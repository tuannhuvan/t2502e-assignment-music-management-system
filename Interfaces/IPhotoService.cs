using CloudinaryDotNet.Actions;

namespace Music_Management_System.Interfaces;

public interface IPhotoService
{
    // Định nghĩa một "hợp đồng" quy định hành vi upload ảnh
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
    Task<DeletionResult> DeletePhotoAsync(string publicId); // chức năng Xóa ảnh qua Public ID của Cloudinary
}