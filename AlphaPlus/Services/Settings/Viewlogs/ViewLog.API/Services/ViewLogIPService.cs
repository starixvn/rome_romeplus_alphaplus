namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    public interface ViewLogIPService
    {
        string FormatIP(string ip);

        string FormatCountryCode(string ip, string country);
    }
}
