using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests;
public class AddMemberBankInfoRequest : BaseQuery
{
    public int MemberId { get; set; }
    public int MemberBankId { get; set; }
    public string MemberBankAccountNumber { get; set; }

    public string AgentBankAccountNumber { get; set; }
    public int AgentBankId { get; set; }
}