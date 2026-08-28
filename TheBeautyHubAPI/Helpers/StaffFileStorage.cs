using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TheBeautyHubAPI.Helpers
{
    public class StaffFileStorage
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".pdf" };
        private const long MaxFileBytes = 5 * 1024 * 1024;
        private const string Folder = "uploads/staff";

        private readonly IWebHostEnvironment _environment;

        public StaffFileStorage(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public Task<string> SavePhotoAsync(IFormFile file) => SaveAsync(file, "Photo");

        public Task<string> SaveAadhaarAsync(IFormFile file) => SaveAsync(file, "Aadhaar card");

        private async Task<string> SaveAsync(IFormFile file, string label)
        {
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? string.Empty;
            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException($"{label} must be a jpg, jpeg, png, webp, gif, or pdf file.");

            if (file.Length <= 0 || file.Length > MaxFileBytes)
                throw new ArgumentException($"{label} file size must be greater than 0 and at most 5 MB.");

            var webRoot = string.IsNullOrWhiteSpace(_environment.WebRootPath)
                ? Path.Combine(_environment.ContentRootPath, "wwwroot")
                : _environment.WebRootPath;

            var directory = Path.Combine(webRoot, "uploads", "staff");
            Directory.CreateDirectory(directory);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(directory, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{Folder}/{fileName}";
        }

        public void DeleteIfLocal(string? path)
        {
            if (string.IsNullOrWhiteSpace(path) || path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return;

            var relative = path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var webRoot = string.IsNullOrWhiteSpace(_environment.WebRootPath)
                ? Path.Combine(_environment.ContentRootPath, "wwwroot")
                : _environment.WebRootPath;

            var fullPath = Path.Combine(webRoot, relative);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
