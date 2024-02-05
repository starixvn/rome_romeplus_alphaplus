namespace Alpha.Main.Site.Features.OTP
{
    using MediatR;

    public class ResetOtpQuery : IRequest<ResetOtpViewModel>
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public string Username { get; set; }
    }
}