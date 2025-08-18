namespace AlphaPlus.CashService.API.Features.Shared.Helpers;

public class FileHelper
{
    public static string ByteToBase64String(byte[] imageBytes)
    {
        if (imageBytes == null || imageBytes.Length == 0)
        {
            return string.Empty;
        }
        var mimeType = GetImageMimeType(imageBytes);
        return $"data:{mimeType};base64,{Convert.ToBase64String(imageBytes)}";
    }

    private static string GetImageMimeType(byte[] bytes)
    {
        if (bytes == null || bytes.Length < 4)
        {
            return "application/octet-stream";
        }

        // PNG
        if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
        {
            return "image/png";
        }

        // JPEG
        if (bytes[0] == 0xFF && bytes[1] == 0xD8)
        {
            return "image/jpeg";
        }

        // GIF
        if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
        {
            return "image/gif";
        }

        // BMP
        if (bytes[0] == 0x42 && bytes[1] == 0x4D)
        {
            return "image/bmp";
        }

        return "application/octet-stream";
    }
}