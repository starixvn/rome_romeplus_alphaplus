
namespace AlphaPlus.Settings.Site.Features.QRCodeMemberSignIn.Requests
{
    using AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class CreateAccountByQRCodeRequest :
        QRCodeBaseQuery,
        IRequest<ResultCreateMemberQRCode>
    {
        public CreateAccountByQRCodeRequest(IUser viewer) : base(viewer)
        {
            SiteDBGroupIp = viewer.SiteDBGroupIp;
        }

        public string UsernameCopyFrom { get; set; }

        public string Username => $"{CustName}{FirstSuffix}{SecondSuffix}{ThirdSuffix}";

        public string FirstName { get; set; }

        public string FirstSuffix { get; set; }

        public string SecondSuffix { get; set; }

        public string ThirdSuffix { get; set; }

        public decimal Credit { get; set; }

        public decimal CreditMin { get; set; }

        public decimal CreditMax { get; set; }

        public bool ShowPageTitle { get; set; }

        public string SiteDBGroupIp { get; }
        public override bool IsValid() =>
            base.IsValid() &&
            !IsInternal &&
            !string.IsNullOrEmpty(UsernameCopyFrom) &&
            Credit <= CreditMax &&
            Credit >= CreditMin;
    }
}