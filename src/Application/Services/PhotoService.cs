using Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file, string type)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                string folder = type?.Trim().ToLower() switch
                {
                    "user" => "users",
                    "event" => "events",
                    _ => throw new ArgumentException($"Invalid type value: {type}")
                };

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = folder
                };

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

