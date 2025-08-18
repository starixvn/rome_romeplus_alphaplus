namespace AlphaPlus.CashService.Site.Features.Promotion.Helpers;

public static class ImageUploadHelper
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

    public static void CreateDirectoryIfNotExisted(string path)
    {
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), path);
        if (!Directory.Exists(uploadPath))
        {
            try
            {
                Directory.CreateDirectory(uploadPath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Cannot create upload directory {uploadPath}: {ex.Message}");
            }
        }
    }

    public static async Task ProcessUploadedImage(IFormFile image, string fileName, string uploadPath)
    {
        if (image == null || image.Length == 0 || !IsValidImageFile(image))
            throw new InvalidOperationException($"Invalid image file upload: File: {image?.FileName}, Size: {image?.Length}, ContentType: {image?.ContentType}");

        var filePath = Path.Combine(uploadPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream);
        await stream.FlushAsync();
        if (OperatingSystem.IsLinux())
        {
            File.SetUnixFileMode(filePath, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.GroupRead | UnixFileMode.OtherRead);
        }
    }

    private static bool IsValidImageFile(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return AllowedExtensions.Contains(extension) && file.ContentType.StartsWith("image/");
    }
}