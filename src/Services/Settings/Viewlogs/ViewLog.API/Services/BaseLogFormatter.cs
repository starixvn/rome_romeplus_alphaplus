using AlphaPlus.Settings.ViewLogs.API.Params;

namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    public interface BaseLogFormatter
    {
        void Init(TransformParam transformParam, bool isHideNickname = false);

        string FormatIP(string ip);

        string GetLogResource(string key);

        string GetProductName(int productId);
        
        string GetBankName(string code);
    }
}