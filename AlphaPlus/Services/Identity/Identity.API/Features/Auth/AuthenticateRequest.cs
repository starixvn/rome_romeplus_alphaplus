
namespace AlphaPlus.Identity.API.Features.Auth
{
    public class AuthenticateRequest
    {
        public int CustId { get; set; }

        public string Password { get; set; }

        public int SubAccId { get; set; }

        public string Nickname { get; set; }

        public bool IsNickname { get; set; }

        public DateTime LoginTime { get; set; }

        public string Language { get; set; }

        public string LoginIp { get; set; }

        public string CountryCode { get; set; }

        public string LoginDomain { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }
    }
}