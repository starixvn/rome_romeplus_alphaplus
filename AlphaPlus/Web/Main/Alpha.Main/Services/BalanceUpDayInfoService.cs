namespace Alpha.Main.Services
{
    using Models.BalanceInfo;

    public interface BalanceUpDayInfoService
    {
        Task<BalanceUpDayInfo> GetBalanceUpDayInfo();
    }
}