namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit;

using Core.Criteria;
using Core.Helpers;
using Domain.Params;
using Fanex.Data;

public class ResetCurrentSportsbookWinAmountCriteria(int siteDbGroupId) : SiteDbBaseCommand(siteDbGroupId)
{
    public ResetCurrentSportsbookWinAmountCriteria(ResetCurrentSportsbookWinAmountParam param) : this(param.SiteDB)
    {
        CustId = param.CustId;
        ProductId = param.ProductId;
        SportType = param.SportType;
        BetType = param.BetType;
        SubAccId = param.SubAccId;
        SubAccName = param.SubAccName;
        UserId = param.UserId;
        IP = param.IP;
        SettingStatus = param.SettingStatus;
    }

    public int CustId { get; }

    [SpParam(Ignored = true)]
    public int ProductId { get; }

    [SpParam(Ignored = true)]
    public int SportType { get; }

    [SpParam(Ignored = true)]
    public int BetType { get; }

    public int SubAccId { get; }

    public string SubAccName { get; }

    public int UserId { get; }

    public string IP { get; }

    public int SettingStatus { get; }

    public override string GetSettingKey()
        => SpNameHelpers.SiteDbPrefix("Acc_CustWinLimitBetReset_CashMember", SiteDbGroupId);

    public override bool IsValid() => CustId > 0;
}