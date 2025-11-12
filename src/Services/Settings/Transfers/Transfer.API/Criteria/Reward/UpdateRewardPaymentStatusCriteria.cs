namespace AlphaPlus.Settings.Transfer.API.Criteria.Reward;

using Core.Criteria;
using Core.Helpers;

internal class UpdateRewardPaymentStatusCriteria(
    string data,
    int creator,
    int subAccId,
    string subAccName,
    string ip,
    string language,
    int siteDbGroupId) : SiteDbBaseCommand(siteDbGroupId)
{
    public string Data { get; } = data;

    public int Creator { get; } = creator;

    public int SubAccId { get; } = subAccId;

    public string SubAccName { get; } = subAccName;

    public string IP { get; } = ip;

    public string Language { get; } = language;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public string PaymentLog { get; set; }

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Acc_RewardPayment_Confirm", SiteDbGroupId);

    public override bool IsValid() => !string.IsNullOrWhiteSpace(Data) && Creator > 0;
}