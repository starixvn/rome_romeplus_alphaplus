namespace Alpha.MemberInfo.Site.Features.QRCodeMemberSignIn.Requests
{
    using Alpha.MemberInfo.Site.Features.QRCodeMemberSignIn.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class GenerateQRCodeRequest : QRCodeBaseQuery, IRequest<QRCodeLinkViewModel>
    {
        public GenerateQRCodeRequest(IUser viewer) : base(viewer)
        {
        }

        public int SearchAccountId { get; set; }

        public string SearchAccountName { get; set; }

        public string Domain { get; set; }

        public override bool IsValid() => 
            base.IsValid() &&
            !IsInternal &&
            SearchAccountId > 0 &&
            !string.IsNullOrEmpty(SearchAccountName) &&
            !string.IsNullOrEmpty(Domain);
    }
}