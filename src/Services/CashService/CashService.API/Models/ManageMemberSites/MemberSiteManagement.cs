namespace AlphaPlus.CashService.API.Models;

public class MemberSiteManagementModel
{
    public int CustId { get; set; }
    public string CountryCode { get; set; }
    public IEnumerable<string> CountryCodes { get; set; }
    public IEnumerable<DomainModel> Domains { get; set; }
    public IEnumerable<MemberSitePermission> GeneralSettings { get; set; }
    public string Mobile { get; set; }
    public string LineId { get; set; }
    public string Announcement { get; set; }
}

public class DomainModel
{
    public string Url { get; set; }
}

public class MemberSitePermission
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Value { get; set; }
}

public class CashServiceManagementDto
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
    public string LineId { get; set; }
    public string Announcement { get; set; }
}

