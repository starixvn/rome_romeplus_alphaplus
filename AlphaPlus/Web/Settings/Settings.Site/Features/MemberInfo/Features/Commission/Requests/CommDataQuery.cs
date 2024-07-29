using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class CommDataQuery : CommissionQuery, IRequest<CommissionViewModel>
{
    public int MasterId { get; set; }

    public override bool IsValid()
        => Id > 0 && ActorLevel.IsMasterOrSuperLevel() && base.IsValid();
}
