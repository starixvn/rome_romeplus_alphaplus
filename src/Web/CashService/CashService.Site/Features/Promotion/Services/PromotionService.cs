namespace AlphaPlus.CashService.Site.Features.Promotion.Services;

using Fanex.Logging;
using Helpers;
using Microsoft.Extensions.Options;
using Models.Configurations;
using Newtonsoft.Json;
using Requests;
using Shared.Models;

public interface IPromotionService
{
    Dictionary<string, IFormFile> ProcessPromotionRequest(AddPromotionRequest request);

    Dictionary<string, IFormFile> ProcessEditPromotionRequest(EditPromotionRequest request);

    Task<ResultModel> ProcessBannerUploadedAsync(Dictionary<string, IFormFile> files);
}

public class PromotionService(ILogger logger, IOptions<UploadImageSettings> options) : IPromotionService
{
    private const string PromotionUploadFolder = "wwwroot/PromotionImages";
    private readonly UploadImageSettings uploadBannerSettings = options.Value;
    private static readonly HttpClient HttpClient = new();

    public Dictionary<string, IFormFile> ProcessEditPromotionRequest(EditPromotionRequest request)
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
        var listResults = new List<ResultModel>();
        foreach (var file in files)
        {
            var result = await UploadBannerResultAsync(file.Value, file.Key, uploadDirectory);
            listResults.Add(result);
        }

        if (listResults.Any(r => r.Code != 0))
        {
            return new ResultModel { Code = 1, Message = "Upload banner failed" };
        }

        if (uploadBannerSettings is not null
            && uploadBannerSettings.IsHttpClientUpload
            && uploadBannerSettings.Locations.Count > 0)
        {
            UploadImageThroughApi(files);
        }
        return new ResultModel(0, string.Empty);
    }

    private void UploadImageThroughApi(Dictionary<string, IFormFile> files)
    {
        foreach (var file in files)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.Value.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
            var fileName = file.Key;
            var locations = uploadBannerSettings.Locations;
            var loggerRef = logger;
            var httpClientRef = HttpClient;

            Task.Run(async () =>
            {
                try
                {
                    await httpClientRef.UploadImageToAllDomainsAsync(
                        loggerRef,
                        locations,
                        fileBytes,
                        fileName);
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync($@"Error uploading {file.Key} through API: {ex.Message}", ex);
                }
            });
        }
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
            file.BannerName = fileName;
            file.Banner = null;
        }

        return fileDictionary;
    }
}