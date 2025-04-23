using Fanex.BetList.Core.Entities;

namespace AlphaPlus.SmartPunter.Services
{
    public interface ISmartPunterBetListMonitor
    {
        void WriteLog(int customerId, IList<ITicket> tickets);
    }
}