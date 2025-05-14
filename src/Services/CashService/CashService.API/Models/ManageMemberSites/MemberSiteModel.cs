namespace AlphaPlus.CashService.API.Models.ManageMemberSites;

public class MemberSiteModel
{
    public int CustId { get; set; }
    public string CountryCode { get; set; }
    public IEnumerable<string> CountryCodes { get; set; }
    public IEnumerable<DomainModel> Domains { get; set; }
    public IEnumerable<MemberSitePermission> GeneralSettings { get; set; }
    public string Mobile { get; set; }
    public string Contacts { get; set; }
    public string Announcement { get; set; }
}

public class UpdateMemberSiteModel : MemberSiteModel
{
    public int SubAccId { get; set; }
    public string IP { get; set; }
    public string Language { get; set; }
}
