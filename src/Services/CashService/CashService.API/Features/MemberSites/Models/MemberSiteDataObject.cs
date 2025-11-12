namespace AlphaPlus.CashService.API.Features.MemberSites.Models;

public class MemberSiteDataObject
{
    public enum GeneralSettingType
    {
        None = 0,
        AllowRegister = 1,
        AllowLogin = 2,
        AllowAll = 3
    }
    public int SettingInfo { get; set; } = 0;

    public bool IsAllowRegister
    {
        get => SettingInfo == (int)GeneralSettingType.AllowRegister ||
               SettingInfo == (int)GeneralSettingType.AllowAll;
    }

    public bool IsAllowLogin
    {
        get => SettingInfo == (int)GeneralSettingType.AllowLogin ||
               SettingInfo == (int)GeneralSettingType.AllowAll;
    }

    public string Mobile { get; set; }
    public string Contacts { get; set; }
    public string Announcement { get; set; }
}
