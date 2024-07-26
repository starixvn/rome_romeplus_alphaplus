using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class MemberCommissionQuery : MemberInfoBaseQuery, IRequest<MemberCommissionViewModel>
{
    public int Id { get; set; }
}
