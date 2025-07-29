using System.Globalization;
using AgencyCash.API.Notifications.Resources;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Notifications.Models;

public class LocalizedMessage(int targetId, string targetName, int creator)
{
    public int TargetId { get; } = targetId;
    public string TargetName { get; } = targetName;
    public int Creator { get; set; } = creator;
    public string En { get; private set; }
    public string Ch { get; private set; }
    public string Cs { get; private set; }
    public string Th { get; private set; }
    public string Vi { get; private set; }
    public string Jp { get; private set; }
    public string Ko { get; private set; }

    public void AssignPersonalMessage(string resourceKey, object[] messageParams = null)
    {
        messageParams ??= Array.Empty<object>();
        En = BuildPersonalMessage(resourceKey, "en-US", messageParams);
        Ch = BuildPersonalMessage(resourceKey, "zh-TW", messageParams);
        Cs = BuildPersonalMessage(resourceKey, "zh-CN", messageParams);
        Th = BuildPersonalMessage(resourceKey, "th-TH", messageParams);
        Vi = BuildPersonalMessage(resourceKey, "vi-VN", messageParams);
        Jp = BuildPersonalMessage(resourceKey, "ja-JP", messageParams);
        Ko = BuildPersonalMessage(resourceKey, "ko-KR", messageParams);
    }

    private static string BuildPersonalMessage(string resourceKey, string culture, object[] messageParams)
    {
        var resourceString = NotificationsResx.ResourceManager.GetString(resourceKey, CultureInfo.CreateSpecificCulture(culture));

        return string.IsNullOrWhiteSpace(resourceString)
            ? string.Empty
            : string.Format(resourceString, messageParams);
    }

    public void AssignToastMessage(string titleResourceKey, string bodyResourceKey, object[] bodyParams = null)
    {
        bodyParams ??= Array.Empty<object>();
        En = BuildToastMessage(titleResourceKey, bodyResourceKey, "en-US", bodyParams);
        Ch = BuildToastMessage(titleResourceKey, bodyResourceKey, "zh-TW", bodyParams);
        Cs = BuildToastMessage(titleResourceKey, bodyResourceKey, "zh-CN", bodyParams);
        Th = BuildToastMessage(titleResourceKey, bodyResourceKey, "th-TH", bodyParams);
        Vi = BuildToastMessage(titleResourceKey, bodyResourceKey, "vi-VN", bodyParams);
        Jp = BuildToastMessage(titleResourceKey, bodyResourceKey, "ja-JP", bodyParams);
        Ko = BuildToastMessage(titleResourceKey, bodyResourceKey, "ko-KR", bodyParams);
    }

    private static string BuildToastMessage(
        string titleResourceKey,
        string bodyResourceKey,
        string culture,
        object[] bodyParams)
    {
        var content = new ToastContentModel
        {
            Title = NotificationsResx.ResourceManager.GetString(titleResourceKey, CultureInfo.CreateSpecificCulture(culture)),
            Body = string.Format(NotificationsResx.ResourceManager.GetString(bodyResourceKey, CultureInfo.CreateSpecificCulture(culture)), bodyParams)
        };

        return JsonHelper.ToJson(content);
    }
}