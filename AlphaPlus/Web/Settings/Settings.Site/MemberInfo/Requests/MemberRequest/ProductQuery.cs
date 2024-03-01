namespace Alpha.MemberInfo.Site.Requests.MemberRequest
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using ViewModels.MemberRequest;

    public class ProductQuery :
        MemberInfoBaseQuery,
        IRequest<ProductsViewModel>
    {
        public ProductQuery(IUser viewer) : base(viewer)
        {
        }

        public int UserSelectedProductId { get; set; }

        public override bool IsValid() => base.IsValid() && CustLevel.IsAgentLevel();
    }
}