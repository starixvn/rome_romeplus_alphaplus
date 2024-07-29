using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class AgentListQuery : MemberInfoBaseQuery, IRequest<IEnumerable<CustInfo>>
{
    public int MasterId { get; set; }

    public override bool IsValid()
        => MasterId > 0 && ActorLevel.IsSuperLevel() && base.IsValid();
}
