using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class CommissionQuery : CommDataQuery, IRequest<CommissionViewModel>
{
    public string DarkModeName => viewer.DarkModeName;

    public override bool IsValid() => ProductId > 0 && CustId > 0 && ActorLevel.IsEqualOrUplineOf(CustLevel);
}
