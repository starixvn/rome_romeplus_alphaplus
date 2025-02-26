using AlphaPlus.Settings.ViewLogs.API.Models;

namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    public interface CustomerService
    {
        Task<CustomerInfo> GetDownlineInfoByUsername(string downlineUsername, int uplineId);
    }
}
