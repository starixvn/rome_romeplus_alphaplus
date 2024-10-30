using AlphaPlus.Utilities.IP.Models;

namespace AlphaPlus.Main.Site.Features.IPInfo
{
    public class IPInfoViewModel(string ip, bool isValid, IPInformationModel ipInfo = null)
    {
        public string IP { get; } = ip;

        public bool IsValid { get; } = isValid;

        public IPInformationModel IPInfo { get; } = ipInfo;
    }
}