namespace AlphaPlus.Settings.Site.Features.QRCodeMemberSignIn.Requests
{
    using AlphaPlus.Settings.Site.Features.QRCodeMemberSignIn.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class QRCodeMemberSignInRequest :
        QRCodeBaseQuery,
        IRequest<QRCodeMemberSignInViewModel>
    {
        public QRCodeMemberSignInRequest(IUser viewer) : base(viewer)
        {
        }

        public string MemberName { get; set; }

        public decimal Credit { get; set; }

        public bool ShowPageTitle { get; set; } = true;

        public string ErrorMessage { get; set; }
    }
}