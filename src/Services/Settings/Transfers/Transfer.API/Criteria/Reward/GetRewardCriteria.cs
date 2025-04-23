using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.Reward;

internal abstract class GetRewardCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    string programIds,
    bool? paymentStatus,
    string username) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public string ProgramIds { get; } = programIds;

    public bool? PaymentStatus { get; } = paymentStatus;

    public string Username { get; } = username;

    public override bool IsValid() => CustId > 0 && FromDate <= ToDate && !string.IsNullOrWhiteSpace(ProgramIds);
}