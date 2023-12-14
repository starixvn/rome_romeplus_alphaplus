using AlphaPlus.Core.Models;
using Alpha.Main.Models.BalanceInfo;

namespace Alpha.Main.Services
{
    public interface BalanceInfoService
    {
        Task<BalanceInfo> GetBalanceInfo(int custId, UserLevel userLevel);
    }
}