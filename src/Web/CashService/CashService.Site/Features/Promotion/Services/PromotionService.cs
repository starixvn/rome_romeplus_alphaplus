namespace AlphaPlus.CashService.Site.Features.Promotion.Services;

using Helpers;
using Newtonsoft.Json;
using Requests;

public interface IPromotionService
{
    Dictionary<string, IFormFile> ProcessPromotionRequest(AddPromotionRequest request);

    Dictionary<string, IFormFile> ProcessPromotionRequest(EditPromotionRequest request);

    void ProcessImageUploads(Dictionary<string, IFormFile> files);
}

public class PromotionService : IPromotionService
{
    public Dictionary<string, IFormFile> ProcessPromotionRequest(EditPromotionRequest request)
    {
        var files = new Dictionary<string, IFormFile>();
        foreach (var content in request.Contents.Where(content => content.Banner != null))
        {
            content.BannerName = FormatBannerName(request.UserId, request.FromDate, content.Language, Path.GetExtension(content.Banner.FileName));
            files.Add(content.BannerName, content.Banner);

            content.Banner = null;
        }

        return files;
    }

    public Dictionary<string, IFormFile> ProcessPromotionRequest(AddPromotionRequest request)
    {
        var files = new Dictionary<string, IFormFile>();
        if (!string.IsNullOrEmpty(request.ProductSettingsJson))
        {
            request.ProductSettings = JsonConvert.DeserializeObject<List<PromotionProductSetting>>(request.ProductSettingsJson);
            request.ProductSettingsJson = null;
        }

        foreach (var content in request.Contents.Where(content => content.Banner != null))
        {
            content.BannerName = FormatBannerName(request.UserId, request.FromDate, content.Language, Path.GetExtension(content.Banner.FileName));
            files.Add(content.BannerName, content.Banner);

            content.Banner = null;
        }

        return files;
    }

    public void ProcessImageUploads(Dictionary<string, IFormFile> files)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                var uploadTasks = files
                .Where(content => content.Value != null)
                .Select(async content =>
                {
                    await ImageUploadHelper.ProcessUploadedImages(content.Value, content.Key);
                });

                await Task.WhenAll(uploadTasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Background image processing failed: {ex.Message}");
            }
        });
    }

    private static string FormatBannerName(int userId, DateTime fromDate, string language, string fileExtension)
        => $"{userId}_{fromDate:yyyyMMdd}_{language}{fileExtension}";
}