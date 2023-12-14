namespace Alpha.Main.Services
{
    public interface ThemeService
    {
        Task WriteLog(int custId, int subAccId, bool isDarkMode, string ip, string location);
    }
}
