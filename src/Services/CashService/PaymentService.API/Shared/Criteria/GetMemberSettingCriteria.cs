using AlphaPlus.PaymentService.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AlphaPlus.PaymentService.API.Payments.Criteria;

public class GetMemberSettingCriteria(int custId, int custDBGroupId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public decimal AgentGivenCredit { get; set; }

    public decimal CreditMembers_GivenCredit { get; set; }

    public decimal MinLimitAmt { get; set; }

    public decimal MaxLimitAmt { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("Age_CashDeposit_MemberSettingSel", custDBGroupId);

    public override bool IsValid() => CustId > 0;
}