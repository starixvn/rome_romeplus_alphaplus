namespace AlphaPlus.CashService.Site.Features.Promotion.Helpers;

public static class ImageUploadHelper
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private const int MaxFileSizeInMB = 5;
    private const string UploadFolder = "wwwroot/PromotionImages";

    public static async Task<string> ProcessUploadedImages(IFormFile image, string fileName)
    {
        if (image == null || image.Length == 0)
            return null;
        try
        {
            // Ensure upload directory exists
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), UploadFolder);

            // Create directory with proper permissions if it doesn't exist
            if (!Directory.Exists(uploadPath))
            {
                try
                {
                    Directory.CreateDirectory(uploadPath);
                    Console.WriteLine($"Created directory: {uploadPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create directory {uploadPath}: {ex.Message}");
                    throw new InvalidOperationException($"Cannot create upload directory {uploadPath}: {ex.Message}");
                }
            }

            // Validate file
            if (!IsValidImageFile(image))
            {
                throw new InvalidOperationException("Invalid image file");
            }

            // Check if file already exists and generate unique name if needed
            var finalFileName = fileName;
            var filePath = Path.Combine(uploadPath, finalFileName);

            // If file exists, generate unique name
            if (File.Exists(filePath))
            {
                var extension = Path.GetExtension(fileName);
                var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var randomString = Path.GetRandomFileName().Replace(".", "").Substring(0, 6);

                finalFileName = $"{nameWithoutExtension}_{timestamp}_{randomString}{extension}";
                filePath = Path.Combine(uploadPath, finalFileName);
            }

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Store relative path for database
            var relativePath = Path.Combine("PromotionImages", finalFileName).Replace("\\", "/");

            return finalFileName;
        }
        catch (Exception ex)
        {
            // Log the error and return empty dictionary
            Console.WriteLine($"Error processing uploaded image: {ex.Message}");
            return null;
        }
    }

    public static async Task<Dictionary<string, string>> ProcessUploadedImages(List<IFormFile> images)
    {
        var processedImages = new Dictionary<string, string>();

        if (images == null || !images.Any())
            return processedImages;

        try
        {
            // Ensure upload directory exists
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), UploadFolder);

            // Create directory with proper permissions if it doesn't exist
            if (!Directory.Exists(uploadPath))
            {
                try
                {
                    Directory.CreateDirectory(uploadPath);
                    Console.WriteLine($"Created directory: {uploadPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create directory {uploadPath}: {ex.Message}");
                    throw new InvalidOperationException($"Cannot create upload directory {uploadPath}: {ex.Message}");
                }
            }

            // Verify directory is writable
            var testFile = Path.Combine(uploadPath, "test.txt");
            try
            {
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
                Console.WriteLine($"Directory {uploadPath} is writable");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Directory {uploadPath} is not writable: {ex.Message}");
                Console.WriteLine($"Current user: {Environment.UserName}");
                Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");

                // Try to get directory permissions info
                try
                {
                    var dirInfo = new DirectoryInfo(uploadPath);
                    Console.WriteLine($"Directory attributes: {dirInfo.Attributes}");
                }
                catch (Exception permEx)
                {
                    Console.WriteLine($"Cannot get directory info: {permEx.Message}");
                }

                throw new InvalidOperationException($"Directory {uploadPath} is not writable: {ex.Message}");
            }

            // Process each image
            for (int i = 0; i < images.Count; i++)
            {
                var image = images[i];
                if (image == null || image.Length == 0)
                    continue;

                // Validate file
                if (!IsValidImageFile(image))
                    continue;

                // Generate unique filename
                var fileName = GenerateUniqueFileName(image.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Store relative path for database with index as key
                var relativePath = Path.Combine("PromotionImages", fileName).Replace("\\", "/");
                processedImages[$"image_{i}"] = relativePath;
            }

            return processedImages;
        }
        catch (Exception ex)
        {
            // Log the error and return empty dictionary
            Console.WriteLine($"Error processing uploaded images: {ex.Message}");
            return processedImages;
        }
    }

    private static bool IsValidImageFile(IFormFile file)
    {
        // Check file size
        if (file.Length > MaxFileSizeInMB * 1024 * 1024)
            return false;

        // Check file extension
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return false;

        // Check MIME type
        if (!file.ContentType.StartsWith("image/"))
            return false;

        return true;
    }

    private static string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var randomString = Path.GetRandomFileName().Replace(".", "").Substring(0, 6);

        return $"promotion_{timestamp}_{randomString}{extension}";
    }
}