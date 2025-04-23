using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

namespace AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount
{
    public class ProblemAccountModel
    {
        public ProblemAccountModel()
        {
        }

        public int CustId { get; set; }

        public string Username2 { get; set; }

        public string NickName => string.IsNullOrEmpty(Username2) ? string.Empty : Username2;

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte Status { get; set; }

        public string StatusNameEN
            => ProblemAccountStatusConstants.GetProblemAccountStatus(Status);

        public string StatusName
            => Resources.ResourceManager.GetString(ProblemAccountStatusConstants.GetProblemAccountStatus(Status));

        public DateTime CreatedDate { get; set; }

        public string CreatedDateStr => CreatedDate.ToLongDateTime24h();

        public int Count { get; set; }

        public bool IsReviewed { get; set; }

        public int ClosedReason { get; set; }

        public string ClosedReasonName => CloseReasonConstants.GetName(ClosedReason);

        public string ReviewStatus => IsReviewed ? Resources.Reviewed : Resources.NotReviewed;

        public bool CanEdit => !IsReviewed;

        public byte Locked { get; set; }

        public bool IsExistedWatchlist { get; set; }

        public int AgentId { get; set; }

        public bool IsDirectMember
            => AgentId == DirectMemberAgentIds.Super || AgentId == DirectMemberAgentIds.Master;
    }
}
