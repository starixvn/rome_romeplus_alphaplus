namespace AlphaPlus.CashService.Site.Features.Promotion.Helpers;

using System.Net.Http.Headers;
using System.Text;
using Fanex.Logging;
using Models.Configurations;
using Newtonsoft.Json;
using Site.Helpers;
using JsonSerializer = System.Text.Json.JsonSerializer;

public static class HttpClientImageUploadExtensions
{
    public static async Task UploadImageToAllDomainsAsync(
        this HttpClient client,
        ILogger logger,
        List<UploadApiSettings> settings,
        IFormFile file,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        if (file == null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException(@"File name must be provided.", nameof(fileName));
        }

        byte[] fileBytes;
        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms, cancellationToken);
            fileBytes = ms.ToArray();
        }

        _ = settings.Select(async setting =>
        {
            var patchUrl = $"{setting.BaseUrl.TrimEnd('/')}/api/tus/{Uri.EscapeDataString(fileName)}";

            try
            {
                await UploadToDomainAsync(client, setting, fileBytes, patchUrl, cancellationToken);
                await logger.InfoAsync($"Successfully uploaded image to {patchUrl}, fileName: {fileName}");
                return (Success: true, Error: null);
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(
                    $"Error uploading image to {patchUrl}, fileName: {fileName}, settings: {JsonConvert.SerializeObject(setting)}: {ex.Message}",
                    ex);
                return (Success: false, Error: ex.Message);
            }
        });
    }

    private static async Task UploadToDomainAsync(
        HttpClient client,
        UploadApiSettings setting,
        byte[] fileBytes,
        string patchUrl,
        CancellationToken cancellationToken)
    {
        var token = await GetTokenAsync(client, setting, cancellationToken);

        using var patchReq = new HttpRequestMessage(HttpMethod.Patch, patchUrl);
        patchReq.Headers.TryAddWithoutValidation("X-Auth", token);
        patchReq.Headers.TryAddWithoutValidation("Tus-Resumable", "1.0.0");
        patchReq.Headers.TryAddWithoutValidation("Upload-Offset", "0");
        patchReq.Content = new ByteArrayContent(fileBytes);
        patchReq.Content.Headers.ContentType = new MediaTypeHeaderValue("application/offset+octet-stream");

        using var patchResp = await client.SendAsync(patchReq, cancellationToken);

        if (!patchResp.IsSuccessStatusCode)
        {
            throw new CustomHttpClientException(
                requestUri: new Uri(patchUrl),
                requestContent: JsonConvert.SerializeObject(setting),
                statusCode: patchResp.StatusCode,
                reasonPhrase: patchResp.ReasonPhrase
            );
        }
    }

    private static async Task<string> GetTokenAsync(HttpClient client, UploadApiSettings setting, CancellationToken cancellationToken)
    {
        var loginUrl = $"{setting.BaseUrl.TrimEnd('/')}/api/login";
        var loginPayload = new
        {
            username = setting.Username,
            password = setting.Password
        };
        var content = new StringContent(JsonSerializer.Serialize(loginPayload), Encoding.UTF8, "application/json");

        using var resp = await client.PostAsync(loginUrl, content, cancellationToken);
        resp.EnsureSuccessStatusCode();

        var token = await resp.Content.ReadAsStringAsync(cancellationToken);
        return token.Trim('"');
    }
}