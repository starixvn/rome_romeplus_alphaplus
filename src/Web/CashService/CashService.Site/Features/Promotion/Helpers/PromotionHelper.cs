namespace AlphaPlus.CashService.Site.Features.Promotion.Helpers;

using Requests;

public static class PromotionHelper
{
    public static Dictionary<string, IFormFile> ProcessUploadedFiles(int userId, List<PromotionLanguageSetting> files)
    {
        var fileDictionary = new Dictionary<string, IFormFile>();
        foreach (var file in files)
        {
            var fileName = FormatBannerName(userId, file.Language, Path.GetExtension(file.Banner.FileName));
            fileDictionary.Add(fileName, file.Banner);
        }

        return fileDictionary;
    }

    private static string FormatBannerName(int userId, string langCode, string fileExtension)
        => $"{userId}_{DateTime.Now:yyyyMMddHHmmsss}_{Guid.NewGuid().ToString("N")[..6]}_{langCode}{fileExtension}";
}