namespace AlphaPlus.CashService.Site.Features.MemberSites.Models;

public class MemberSiteManagementModel
{
    public IEnumerable<string> CountryCodes { get; set; }
    public IEnumerable<Domain> Domains { get; set; } = new List<Domain>();
    public IEnumerable<GeneralSetting> GeneralSettings { get; set; } = new List<GeneralSetting>();
    public string Mobile { get; set; } = string.Empty;
    public string Contacts { get; set; } = string.Empty;
    public string Announcement { get; set; } = string.Empty;
    public int IsCashServiceEnabled { get; set; }
}

public class UpdateMemberSiteManagementModel : MemberSiteManagementModel
{
    public int CustId { get; set; }
    public int SubAccId { get; set; }
    public string IP { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int SiteDB { get; set; }
}

public class Domain
{
    public string Url { get; set; } = string.Empty;
} 
public class GeneralSetting
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Value { get; set; }
}


