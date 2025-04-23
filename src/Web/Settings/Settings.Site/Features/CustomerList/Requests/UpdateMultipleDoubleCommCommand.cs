using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    public class UpdateMultipleDoubleCommCommand : CustomerStatusQueryBase, IRequest<IDictionary<string, Result>>
    {
        public override bool IsValid()
        => !string.IsNullOrEmpty(SearchAccountIds) && !string.IsNullOrWhiteSpace(Usernames) && CanUpdateAccount && !IsInternal && (IsDirectMember || viewer.LevelId == SearchAccountRoleId + 1);

        public string SearchAccountIds { get; set; }

        public bool EnabledDoubleComm { get; set; }

        public string Usernames { get; set; }

        public new int AgentId { get; set; }

        public new bool IsDirectMember
            => (ViewerLevel.IsSuperLevel() && AgentId == DirectMemberAgentIds.Super) || (ViewerLevel.IsMasterLevel() && AgentId == DirectMemberAgentIds.Master);
    }
}