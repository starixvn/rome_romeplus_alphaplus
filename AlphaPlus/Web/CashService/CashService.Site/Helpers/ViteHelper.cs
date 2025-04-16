using System.Text.Json;
using System.Text.Json.Serialization;

namespace AlphaPlus.CashService.Site.Helpers;

public class ManifestEntry
{
    [JsonPropertyName("file")]
    public string File { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("src")]
    public string Src { get; set; }

    [JsonPropertyName("isEntry")]
    public bool IsEntry { get; set; }

    [JsonPropertyName("css")]
    public string[] Css { get; set; }
}

public static class ViteHelper
{
    private static IConfiguration _configuration;
    private static IWebHostEnvironment _environment;
    private static Dictionary<string, ManifestEntry> _manifestData;

    public static void Initialize(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
        LoadManifest(_environment.IsProduction());
    }

    private static void LoadManifest(bool isProduction)
    {
        if (isProduction)
        {
            // Đọc manifest từ ClientApp/dist/.vite/manifest.json
            var clientAppPath = Path.Combine(_environment.ContentRootPath, "ClientApp", "dist", ".vite", "manifest.json");
            if (File.Exists(clientAppPath))
            {
                var manifestContent = File.ReadAllText(clientAppPath);
                _manifestData = JsonSerializer.Deserialize<Dictionary<string, ManifestEntry>>(manifestContent);
            }
        }
    }

    public static string GetViteAsset(string entryName)
    {
        if (_environment.IsDevelopment())
        {
            return $"http://localhost:5173/src/pages/{entryName}/index.tsx";
        }

        var key = $"src/pages/{entryName}/index.tsx";
        return _manifestData.TryGetValue(key, out var asset) 
            ? $"/dist/{asset.File}" 
            : string.Empty;
    }

    public static string GetViteStyle(string entryName)
    {
        if (_environment.IsDevelopment())
        {
            return string.Empty;
        }

        var key = $"src/pages/{entryName}/index.tsx";
        if (_manifestData.TryGetValue(key, out var asset) && asset.Css != null && asset.Css.Length > 0)
        {
            return $"/dist/{asset.Css[0]}";
        }
        return string.Empty;
    }
} 