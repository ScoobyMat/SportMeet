using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file, string type);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
