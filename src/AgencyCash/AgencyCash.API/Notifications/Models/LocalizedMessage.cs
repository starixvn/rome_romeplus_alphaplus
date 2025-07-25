using System.Globalization;
using AgencyCash.API.Notifications.Resources;

namespace AgencyCash.API.Notifications.Models;

public class LocalizedMessage(int agentId, int custId, string agentName)
{
    public int AgentId { get; } = agentId;
    public int CustId { get; } = custId;
    public string AgentName { get; } = agentName;
    public string En { get; private set; }
    public string Ch { get; private set; }
    public string Cs { get; private set; }
    public string Th { get; private set; }
    public string Vi { get; private set; }
    public string Jp { get; private set; }
    public string Ko { get; private set; }

    public void AssignLocalizedMessage(string resourceKey, object[] messageParams = null)
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