namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    using AlphaPlus.Settings.ViewLogs.API.Models;

    public interface CustomerService
    {
        CustomerInfo GetDownlineInfoByUsername(string downlineUsername, int uplineId);
    }
}
