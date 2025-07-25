using System.Globalization;
using AgencyCash.API.Notifications.Resources;

namespace AgencyCash.API.Notifications.Models;

public class LocalizedMessage
{
    public string En { get; internal set; }
    public string Ch { get; internal set; }
    public string Cs { get; internal set; }
    public string Th { get; internal set; }
    public string Vi { get; internal set; }
    public string Jp { get; internal set; }
    public string Ko { get; internal set; }

    public void AssignLocalizedMessage(string resourceKey, object[] messageParams)
    {
        messageParams ??= Array.Empty<object>();
        En = GetMessage(resourceKey, "en-US", messageParams);
        Ch = GetMessage(resourceKey, "zh-TW", messageParams);
        Cs = GetMessage(resourceKey, "zh-CN", messageParams);
        Th = GetMessage(resourceKey, "th-TH", messageParams);
        Vi = GetMessage(resourceKey, "vi-VN", messageParams);
        Jp = GetMessage(resourceKey, "ja-JP", messageParams);
        Ko = GetMessage(resourceKey, "ko-KR", messageParams);
    }

    private static string GetMessage(string resourceKey, string culture, object[] messageParams)
    {
        var resourceString = NotificationsResx.ResourceManager.GetString(resourceKey, CultureInfo.CreateSpecificCulture(culture));

        return string.IsNullOrWhiteSpace(resourceString)
            ? string.Empty
            : string.Format(resourceString, messageParams);
    }
}