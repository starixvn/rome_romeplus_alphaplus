using AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.ViewModels;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.Requests
{
    public class ShowQRCodeRequest :
        QRCodeBaseQuery,
        IRequest<ShowQRCodeViewModel>
    {
        public ShowQRCodeRequest() : this(null) { }

        public ShowQRCodeRequest(IUser viewer) : base(viewer)
        {
        }

        public int SearchAccountId { get; set; }

        public string SearchAccountName { get; set; }

        public string FirstName { get; set; }

        public string RawPassword { get; set; }

        public decimal Credit { get; set; }

        public decimal CreditMin { get; set; }

        public decimal CreditMax { get; set; }

        public int CustIdCopyFrom { get; set; }

        public bool CreateMode { get; set; }

        public bool ShowPageTitle { get; set; } = true;

        public override bool IsValid() =>
            base.IsValid() &&
            !IsInternal &&
            SearchAccountId > 0 &&
            !string.IsNullOrEmpty(SearchAccountName) &&
            Credit <= CreditMax &&
            Credit >= CreditMin;
    }
}