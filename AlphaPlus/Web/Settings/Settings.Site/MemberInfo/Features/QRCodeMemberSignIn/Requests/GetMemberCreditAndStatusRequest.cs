namespace AlphaPlus.Settings.Site.Features.QRCodeMemberSignIn.Requests
{
    using AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class GetMemberCreditAndStatusRequest : QRCodeBaseQuery, IRequest<CreditAndStatusModel>
    {
        public GetMemberCreditAndStatusRequest(IUser viewer) : base(viewer)
        {
        }

        public string SearchAccountName { get; set; }

        public override bool IsValid() => 
            base.IsValid() && 
            !string.IsNullOrEmpty(SearchAccountName);
    }
}