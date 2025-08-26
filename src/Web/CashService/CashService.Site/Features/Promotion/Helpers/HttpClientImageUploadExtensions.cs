namespace AlphaPlus.CashService.Site.Features.Promotion.Helpers;

using System.Net.Http.Headers;
using System.Text;
using Fanex.Logging;
using Models.Configurations;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

public static class HttpClientImageUploadExtensions
{
    public static async Task UploadImageToAllDomainsAsync(
        this HttpClient client,
        ILogger logger,
        List<UploadApiSettings> settings,
        byte[] fileBytes,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        foreach (var setting in settings)
        {
            try
            {
                await UploadToDomainAsync(client, logger, setting, fileBytes, fileName, cancellationToken);
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(
                    $"Error uploading image with fileName: {fileName}, settings: {JsonConvert.SerializeObject(setting)}: {ex.Message}",
                    ex);
            }
        }
    }

    private static async Task UploadToDomainAsync(
        HttpClient client,
        ILogger logger,
        UploadApiSettings setting,
        byte[] fileBytes,
        string fileName,
        CancellationToken cancellationToken)
    {
        var token = await GetTokenAsync(client, setting, cancellationToken);
        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException(@$"Upload to {setting.BaseUrl} failed Token is empty or invalid.");

        var createUrl = $"{setting.BaseUrl.TrimEnd('/')}/api/tus/{Uri.EscapeDataString(fileName)}";
        await logger.InfoAsync($"[TUS] Create upload: {createUrl}");

        using (var createReq = new HttpRequestMessage(HttpMethod.Post, createUrl))
        {
            createReq.Headers.TryAddWithoutValidation("X-Auth", token);
            createReq.Headers.TryAddWithoutValidation("Tus-Resumable", "1.0.0");
            createReq.Headers.TryAddWithoutValidation("Upload-Length", fileBytes.LongLength.ToString());

            using var createResp = await client.SendAsync(createReq, cancellationToken);
            createResp.EnsureSuccessStatusCode();
        }

        var patchUrl = $"{setting.BaseUrl.TrimEnd('/')}/api/tus/{Uri.EscapeDataString(fileName)}";
        await logger.InfoAsync($"[TUS] Upload bytes: {patchUrl}");

        using (var patchReq = new HttpRequestMessage(new HttpMethod("PATCH"), patchUrl))
        {
            patchReq.Headers.TryAddWithoutValidation("X-Auth", token);
            patchReq.Headers.TryAddWithoutValidation("Tus-Resumable", "1.0.0");
            patchReq.Headers.TryAddWithoutValidation("Upload-Offset", "0");
            patchReq.Content = new ByteArrayContent(fileBytes);
            patchReq.Content.Headers.ContentType = new MediaTypeHeaderValue("application/offset+octet-stream");

            using var patchResp = await client.SendAsync(patchReq, cancellationToken);
            patchResp.EnsureSuccessStatusCode();
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