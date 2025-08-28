namespace AlphaPlus.CashService.Site.Features.Promotion.Models.Configurations;

public class UploadImageSettings
{
    public bool IsHttpClientUpload { get; set; }

    public List<UploadApiSettings> Locations { get; set; }
}

public class UploadApiSettings
{
    public string BaseUrl { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}