namespace AlphaPlus.CashService.Site.Features.Promotion.Services;

using Fanex.Logging;
using Helpers;
using Newtonsoft.Json;
using Requests;
using Shared.Models;

public interface IPromotionService
{
    Dictionary<string, IFormFile> ProcessPromotionRequest(AddPromotionRequest request);

    Dictionary<string, IFormFile> ProcessPromotionRequest(EditPromotionRequest request);

    Task<ResultModel> ProcessBannerUploadedAsync(Dictionary<string, IFormFile> files);

    void RemoveFailedImageAsync(string[] fileNames);
}

public class PromotionService(ILogger logger) : IPromotionService
{
    private const string PromotionUploadFolder = "wwwroot/PromotionImages";

    public Dictionary<string, IFormFile> ProcessPromotionRequest(EditPromotionRequest request)
    {
        var files = ProcessUploadedFiles(request.UserId,
            request.Contents.Where(content => content.Banner != null).ToList());

        return files;
    }

    public Dictionary<string, IFormFile> ProcessPromotionRequest(AddPromotionRequest request)
    {
        if (!string.IsNullOrEmpty(request.ProductSettingsJson))
        {
            request.ProductSettings = JsonConvert.DeserializeObject<List<PromotionProductSetting>>(request.ProductSettingsJson);
            request.ProductSettingsJson = null;
        }

        var files = ProcessUploadedFiles(request.UserId,
            request.Contents.Where(content => content.Banner != null).ToList());

        return files;
    }

    public async Task<ResultModel> ProcessBannerUploadedAsync(Dictionary<string, IFormFile> files)
    {
        ImageUploadHelper.CreateDirectoryIfNotExisted(PromotionUploadFolder);

        var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), PromotionUploadFolder);

        var uploadTasks = files
            .Where(f => f.Value != null)
            .Select(f => UploadBannerResultAsync(f.Value, f.Key, uploadDirectory));

        var listResults = await Task.WhenAll(uploadTasks);
        if (listResults.Any(r => r.Code != 0))
        {
            ImageUploadHelper.DeleteImages(listResults.Select(i => i.Data?.ToString()).ToArray(), PromotionUploadFolder);
            return new ResultModel { Code = 1, Message = "Upload banner failed" };
        }
        return new ResultModel(0, string.Empty);
    }

    public void RemoveFailedImageAsync(string[] fileNames)
    {
        ImageUploadHelper.DeleteImages(fileNames, PromotionUploadFolder);
    }

    private async Task<ResultModel> UploadBannerResultAsync(IFormFile file, string fileName, string directory)
    {
        var result = new ResultModel { Code = 0 };
        try
        {
            await ImageUploadHelper.ProcessUploadedImage(file, fileName, directory);
        }
        catch (Exception e)
        {
            result.Code = 1;
            result.Data = fileName;
            result.Message = e.Message;
            await logger.ErrorAsync($"Upload {file.FileName} to {directory} failed. Error message: {e.Message}", e);
        }
        return result;
    }

    private static string FormatBannerName(int userId, string langCode, string fileExtension)
        => $"{userId}_{DateTime.Now:yyyyMMddHHmmsss}_{Guid.NewGuid().ToString("N")[..6]}_{langCode}{fileExtension}";

    private static Dictionary<string, IFormFile> ProcessUploadedFiles(int userId, List<PromotionLanguageSetting> files)
    {
        var fileDictionary = new Dictionary<string, IFormFile>();
        foreach (var file in files)
        {
            var fileName = FormatBannerName(userId, file.Language, Path.GetExtension(file.Banner.FileName));
            fileDictionary.Add(fileName, file.Banner);
            file.Banner = null;
        }

        return fileDictionary;
    }
}