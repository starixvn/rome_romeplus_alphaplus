using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.NovaCasino.Models
{
    public class NovaCasinoSSOResult : Result
    {
        public string LaunchUrl { get; set; }

        public string[] CustIds { get; set; }
    }
}