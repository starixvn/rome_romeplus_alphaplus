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
        {
            throw new InvalidOperationException(@$"Upload to {setting.BaseUrl} failed Token is empty or invalid.");
        }

        var baseUrl = setting.BaseUrl.TrimEnd('/');
        var escapedFileName = Uri.EscapeDataString(fileName);

        await CreateUploadSessionAsync(client, logger, baseUrl, escapedFileName, token, fileBytes.LongLength, cancellationToken);

        await UploadFileContentAsync(client, logger, baseUrl, escapedFileName, token, fileBytes, cancellationToken);
    }

    private static async Task CreateUploadSessionAsync(
        HttpClient client,
        ILogger logger,
        string baseUrl,
        string escapedFileName,
        string token,
        long fileSize,
        CancellationToken cancellationToken)
    {
        var createUrl = $"{baseUrl}/api/tus/{escapedFileName}";

        using var createReq = new HttpRequestMessage(HttpMethod.Post, createUrl);
        AddTusHeaders(createReq, token, fileSize: fileSize);

        using var createResp = await client.SendAsync(createReq, cancellationToken);
        await ValidateResponseAsync(createResp, createUrl, "Create upload");

        await logger.InfoAsync($"[TUS] Create upload: {createUrl} successful!!!");
    }

    private static async Task UploadFileContentAsync(
        HttpClient client,
        ILogger logger,
        string baseUrl,
        string escapedFileName,
        string token,
        byte[] fileBytes,
        CancellationToken cancellationToken)
    {
        var patchUrl = $"{baseUrl}/api/tus/{escapedFileName}";

        using var patchReq = new HttpRequestMessage(new HttpMethod("PATCH"), patchUrl);
        AddTusHeaders(patchReq, token, uploadOffset: 0);
        patchReq.Content = new ByteArrayContent(fileBytes);
        patchReq.Content.Headers.ContentType = new MediaTypeHeaderValue("application/offset+octet-stream");

        using var patchResp = await client.SendAsync(patchReq, cancellationToken);
        await ValidateResponseAsync(patchResp, patchUrl, "Upload bytes");

        await logger.InfoAsync($"[TUS] Upload bytes: {patchUrl}, upload successful!!!");
    }

    private static void AddTusHeaders(HttpRequestMessage request, string token, long? fileSize = null, long? uploadOffset = null)
    {
        request.Headers.TryAddWithoutValidation("X-Auth", token);
        request.Headers.TryAddWithoutValidation("Tus-Resumable", "1.0.0");

        if (fileSize.HasValue)
        {
            request.Headers.TryAddWithoutValidation("Upload-Length", fileSize.Value.ToString());
        }

        if (uploadOffset.HasValue)
        {
            request.Headers.TryAddWithoutValidation("Upload-Offset", uploadOffset.Value.ToString());
        }
    }

    private static Task ValidateResponseAsync(
        HttpResponseMessage response,
        string requestUrl,
        string operation)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new CustomHttpClientException(
                requestUri: new Uri(requestUrl),
                requestContent: operation,
                statusCode: response.StatusCode,
                reasonPhrase: response.ReasonPhrase
            );
        }

        return Task.CompletedTask;
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