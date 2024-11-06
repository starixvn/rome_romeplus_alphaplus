using MediatR;

namespace AlphaPlus.Main.Site.Features.OTP
{
    public class ResetOtpQuery : IRequest<ResetOtpViewModel>
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public string Username { get; set; }
    }
}