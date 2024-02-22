namespace AlphaPlus.Main.Site.Features.OTP
{
    public class ChangeOtpStatusParam
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public int ActorId { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public bool IsEnable { get; set; }
    }
}