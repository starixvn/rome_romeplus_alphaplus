using System.Globalization;
using AgencyCash.API.Notifications.Resources;
using AgencyCash.API.Shared.Constants;
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
        En = BuildMessage(resourceKey, Language.English, messageParams);
        Ch = BuildMessage(resourceKey, Language.ChineseTraditional, messageParams);
        Cs = BuildMessage(resourceKey, Language.ChineseSimplified, messageParams);
        Th = BuildMessage(resourceKey, Language.Thai, messageParams);
        Vi = BuildMessage(resourceKey, Language.Vietnamese, messageParams);
        Jp = BuildMessage(resourceKey, Language.Japanese, messageParams);
        Ko = BuildMessage(resourceKey, Language.Korean, messageParams);
    }

    public void AssignToastMessage(string titleResourceKey, string bodyResourceKey, object[] bodyParams = null)
    {
        bodyParams ??= Array.Empty<object>();
        En = BuildToastMessage(titleResourceKey, bodyResourceKey, Language.English, bodyParams);
        Ch = BuildToastMessage(titleResourceKey, bodyResourceKey, Language.ChineseTraditional, bodyParams);
        Cs = BuildToastMessage(titleResourceKey, bodyResourceKey, Language.ChineseSimplified, bodyParams);
        Th = BuildToastMessage(titleResourceKey, bodyResourceKey, Language.Thai, bodyParams);
        Vi = BuildToastMessage(titleResourceKey, bodyResourceKey, Language.Vietnamese, bodyParams);
        Jp = BuildToastMessage(titleResourceKey, bodyResourceKey, Language.Japanese, bodyParams);
        Ko = BuildToastMessage(titleResourceKey, bodyResourceKey, Language.Korean, bodyParams);
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
            Body = BuildMessage(bodyResourceKey, culture, bodyParams)
        };

        return JsonHelper.ToJson(content);
    }

    private static string BuildMessage(string resourceKey, string culture, object[] messageParams)
    {
        var resourceString = NotificationsResx.ResourceManager.GetString(resourceKey, CultureInfo.CreateSpecificCulture(culture));

        return string.IsNullOrWhiteSpace(resourceString)
            ? string.Empty
            : string.Format(resourceString, messageParams);
    }
}