using System.Globalization;
using AgencyCash.API.Notifications.Resources;
using AgencyCash.API.Shared.Constants;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Notifications.Models;

public class LocalizedMessage(
    int targetId,
    string targetName,
    int creator,
    string titleMessageResourceKey,
    string messageResourceKey,
    object[] messageParams = null)
{
    public int TargetId { get; } = targetId;
    public string TargetName { get; } = targetName;
    public int Creator { get; } = creator;
    public string TitleMessageResourceKey { get; } = titleMessageResourceKey;
    public string MessageResourceKey { get; } = messageResourceKey;
    public object[] MessageParams { get; } = messageParams ?? Array.Empty<object>();
    public string En { get; private set; }
    public string Ch { get; private set; }
    public string Cs { get; private set; }
    public string Th { get; private set; }
    public string Vi { get; private set; }
    public string Jp { get; private set; }
    public string Ko { get; private set; }

    public void AssignPersonalMessage()
    {
        En = BuildMessage(Language.English);
        Ch = BuildMessage(Language.ChineseTraditional);
        Cs = BuildMessage(Language.ChineseSimplified);
        Th = BuildMessage(Language.Thai);
        Vi = BuildMessage(Language.Vietnamese);
        Jp = BuildMessage(Language.Japanese);
        Ko = BuildMessage(Language.Korean);
    }

    public void AssignToastMessage()
    {
        En = BuildToastMessage(Language.English);
        Ch = BuildToastMessage(Language.ChineseTraditional);
        Cs = BuildToastMessage(Language.ChineseSimplified);
        Th = BuildToastMessage(Language.Thai);
        Vi = BuildToastMessage(Language.Vietnamese);
        Jp = BuildToastMessage(Language.Japanese);
        Ko = BuildToastMessage(Language.Korean);
    }

    private string BuildToastMessage(string language)
    {
        var culture = CultureInfo.CreateSpecificCulture(language);

        return JsonHelper.ToJson(new ToastContentModel
        {
            Title = NotificationsResx.ResourceManager.GetString(TitleMessageResourceKey, culture),
            Body = BuildMessage(culture)
        });
    }

    private string BuildMessage(string language)
        => BuildMessage(CultureInfo.CreateSpecificCulture(language));

    private string BuildMessage(CultureInfo culture)
    {
        var resourceString = NotificationsResx.ResourceManager.GetString(MessageResourceKey, culture);

        return string.IsNullOrWhiteSpace(resourceString)
            ? string.Empty
            : string.Format(resourceString, MessageParams);
    }
}