using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.MemberRequest;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.MemberRequest
{
    public class ProductQuery :
        MemberInfoBaseQuery,
        IRequest<ProductsViewModel>
    {
        public ProductQuery() : this(null) { }

        public ProductQuery(IUser viewer) : base(viewer)
        {
        }

        public int UserSelectedProductId { get; set; }

        public override bool IsValid() => base.IsValid() && CustLevel.IsAgentLevel();
    }
}