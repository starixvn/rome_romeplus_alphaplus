using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount
{
    public class IndexQuery : BaseQuery, IRequest<IndexViewModel>
    {
        public bool MobileView { get; set; }

        public bool IsEnabledCustomerDateLimit => userSession.GetProperty<DateTime>("CustomerDateLimit") != default;

        public override bool IsValid() => base.IsValid() && ViewMemberInfo;
    }
}