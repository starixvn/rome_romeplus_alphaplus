namespace AlphaPlus.Identity.API.Features.Auth
{
    public class CheckInRequest
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string NetSessionId { get; set; }

        public int ServerId { get; set; }

        public string IP { get; set; }
    }
}