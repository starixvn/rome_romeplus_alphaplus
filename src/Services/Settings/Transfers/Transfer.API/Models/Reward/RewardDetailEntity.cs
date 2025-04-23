using AlphaPlus.Shared.AppResources.Settings.Transfer;

namespace AlphaPlus.Settings.Transfer.API.Models.Reward;

public class RewardDetailEntity
{
    public DateTime CreatedDate { get; init; }

    public int ProgramId { get; init; }

    public string ProgramName { get; set; }

    public int CustId { get; init; }

    public string MemberUsername { get; init; }

    public decimal MemberAmount { get; init; }

    public bool MemberPaymentStatus { get; init; }

    public DateTime? MemberPaymentDate { get; init; }

    public byte? MemberFeedback { get; init; }

    public string MemberFeedbackText
        => MemberFeedback == 0 ? Resources.Received : MemberFeedback == 1 ? Resources.NotReceived : string.Empty;

    public int Recommend { get; init; }

    public string AgentUsername { get; init; }

    public bool AgentPaymentStatus { get; init; }

    public DateTime? AgentPaymentDate { get; init; }

    public int MRecommend { get; init; }

    public string MasterUsername { get; init; }

    public bool MasterPaymentStatus { get; init; }

    public DateTime? MasterPaymentDate { get; init; }

    public bool IsDirectMember { get; init; }
}