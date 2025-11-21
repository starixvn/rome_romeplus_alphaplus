namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

using Core.Criteria;
using Core.Helpers;

public class GetMemberTransferInfoCriteria(int custId, bool allowedTotalBalanceTransfer) : SiteDbBaseCommand
{
    public int CustId { get; } = custId;

    public bool IsAllowTotalBalance { get; } = allowedTotalBalanceTransfer;

    public string Username { get; set; }

    public decimal CashBalance { get; set; }

    public decimal Oustanding { get; set; }

    public decimal GivenCredit { get; set; }

    public decimal BetCredit { get; set; }

    public decimal AmtTransferred { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime? MaxTransDate { get; set; }

    public decimal Balance { get; set; }

    public decimal FutureBetCredit { get; set; }

    public decimal PendingTransfer3rd { get; set; }

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_Transfer_SelMemberInfo_Yesterday_ForDBTrans", SiteDbGroupId);

    public override bool IsValid() => CustId > 0;
}