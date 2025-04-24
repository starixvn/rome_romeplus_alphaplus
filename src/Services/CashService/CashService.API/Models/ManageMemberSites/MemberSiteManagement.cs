using System.ComponentModel.DataAnnotations;

namespace AlphaPlus.CashService.API.Models;

public class MemberSiteManagementModel
{
    // Domain settings
    public List<DomainModel> Domains { get; set; } = new();
    public List<MemberSitePermission> GeneralSettings { get; set; } = new();
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

    public int SettingType { get; set; } = 0;

    public bool IsAllowRegister
    {
        get => SettingType == (int)GeneralSettingType.AllowRegister ||
               SettingType == (int)GeneralSettingType.AllowAll;
    }

    public bool IsAllowLogin
    {
        get => SettingType == (int)GeneralSettingType.AllowLogin ||
               SettingType == (int)GeneralSettingType.AllowAll;
    }

    public string Mobile { get; set; }
    public string LineId { get; set; }
    public string Announcement { get; set; }
}

