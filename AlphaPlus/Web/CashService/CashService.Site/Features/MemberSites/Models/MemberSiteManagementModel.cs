namespace AlphaPlus.CashService.Site.Features.MemberSites.Models;

public class MemberSiteManagementModel
{
    public IEnumerable<Domain> Domains { get; set; } = new List<Domain>();
    public IEnumerable<GeneralSetting> GeneralSettings { get; set; } = new List<GeneralSetting>();
    // public bool AllowRegistration { get; set; }
    // public bool AllowDeposit { get; set; }
    public string Mobile { get; set; } = string.Empty;
    public string LineId { get; set; } = string.Empty;
    public string Announcement { get; set; } = string.Empty;
}

public class Domain
{
    public string Url { get; set; } = string.Empty;
    public string QrCode { get; set; } = string.Empty;
} 
public class GeneralSetting
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Value { get; set; }
}


