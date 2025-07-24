using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System;

namespace AlphaPlus.CashService.Site.Features.Promotion.Helpers;

public static class ImageUploadHelper
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private const int MaxFileSizeInMB = 5;
    private const string UploadFolder = "wwwroot/PromotionImages";

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