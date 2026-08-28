using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TheBeautyHubCore.Constants;

namespace TheBeautyHubAPI.Helpers
{
    public class ServicePhotoStorage
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private const long MaxFileBytes = 5 * 1024 * 1024;
        private const string Folder = "uploads/services";

        private readonly IWebHostEnvironment _environment;

        public ServicePhotoStorage(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? string.Empty;
            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException(ApiMessages.Common.InvalidImageType("photo"));

            if (file.Length <= 0 || file.Length > MaxFileBytes)
                throw new ArgumentException(ApiMessages.Common.FileTooLargeFor("photo"));

            var webRoot = string.IsNullOrWhiteSpace(_environment.WebRootPath)
                ? Path.Combine(_environment.ContentRootPath, "wwwroot")
                : _environment.WebRootPath;

            var directory = Path.Combine(webRoot, "uploads", "services");
            Directory.CreateDirectory(directory);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(directory, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{Folder}/{fileName}";
        }

        public void DeleteIfLocal(string? photoPath)
        {
            if (string.IsNullOrWhiteSpace(photoPath) || photoPath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return;

            var relative = photoPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var webRoot = string.IsNullOrWhiteSpace(_environment.WebRootPath)
                ? Path.Combine(_environment.ContentRootPath, "wwwroot")
                : _environment.WebRootPath;

            var fullPath = Path.Combine(webRoot, relative);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
