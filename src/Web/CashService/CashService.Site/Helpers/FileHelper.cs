namespace AlphaPlus.CashService.Site.Helpers;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

public static class FileHelper
{
    public static async Task<byte[]> CompressAndConvertToBase64Async(IFormFile file, int quality = 70)
    {
        if (file == null || file.Length == 0)
            return null;

        await using var inputStream = file.OpenReadStream();
        using var image = await Image.LoadAsync(inputStream);

        using var outputStream = new MemoryStream();

        var encoder = new JpegEncoder
        {
            Quality = quality // 0–100
        };

        await image.SaveAsJpegAsync(outputStream, encoder);

        return outputStream.ToArray();
    }
}