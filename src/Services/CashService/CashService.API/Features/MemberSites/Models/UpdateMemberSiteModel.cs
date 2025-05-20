namespace AlphaPlus.CashService.API.Features.MemberSites.Models;
public class UpdateMemberSiteModel : MemberSiteModel
{
    public int SubAccId { get; set; }
    public string IP { get; set; }
    public string Language { get; set; }
    public int UserId { get; set; }
    public int SiteDB { get; set; }
}

