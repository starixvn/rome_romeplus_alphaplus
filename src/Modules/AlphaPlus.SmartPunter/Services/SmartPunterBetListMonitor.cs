using AlphaPlus.Core.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.SmartPunter.Services
{
    public class SmartPunterBetListMonitor(
        IUserSession userSession,
        ISmartPunterService smartPunterService,
        IPHelper ipHelper) : ISmartPunterBetListMonitor
    {
        private readonly IUserSession _userSession = userSession ?? UserSession.Current;
        private readonly ISmartPunterService _smartPunterService = smartPunterService;
        private readonly IPHelper _ipHelper = ipHelper;

        public void WriteLog(int customerId, IList<ITicket> tickets)
        {
            int userId = _userSession.AdminId;

            if (!_userSession.IsInternal || !_smartPunterService.CheckInternalAdmin(userId))
            {
                return;
            }

            bool isSmartPunter = false;
            bool hasDownlineSmartPunter = false;
            var customer = _smartPunterService.CheckSmartPunter(customerId);

            if (customer != null)
            {
                isSmartPunter = customer.IsSmartPunter;
                hasDownlineSmartPunter = customer.HasDownlineSmartPunter;
            }

            if (!isSmartPunter && !hasDownlineSmartPunter)
            {
                return;
            }

            bool isRestricted = !_smartPunterService.HasSmartPunterPermission(userId);

            if (tickets == null || tickets.Count == 0)
            {
                if (isSmartPunter)
                {
                    _smartPunterService.WriteLogNoData(customerId, isRestricted, userId, _ipHelper.GetClientIP());
                }
                else if (hasDownlineSmartPunter)
                {
                    _smartPunterService.WriteLogNoData(null, isRestricted, userId, _ipHelper.GetClientIP());
                }
            }
            else
            {
                string userNames = GetUserNamesFromTickets(tickets);

                if (isSmartPunter)
                {
                    _smartPunterService.WriteLog(userNames, isRestricted, userId, _ipHelper.GetClientIP());
                }
                else if (hasDownlineSmartPunter)
                {
                    _smartPunterService.CheckSmartPunterAndWriteLog(userNames, isRestricted, userId, _ipHelper.GetClientIP());
                }
            }
        }

        private string GetUserNamesFromTickets(IList<ITicket> tickets)
        {
            return string.Join(",", tickets.GroupBy(grp => grp.UserName).Select(grp => grp.Key));
        }
    }
}