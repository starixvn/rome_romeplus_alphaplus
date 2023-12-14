using AlphaPlus.Core.Models;
using MediatR;
using Alpha.Main.Site.ViewModels.Dashboard.BalanceInfo;

namespace Alpha.Main.Site.Requests.Dashboard.BalanceInfo
{
    public class BalanceInfoQuery : IRequest<BalanceInfoViewModel>
    {
        public BalanceInfoQuery(IUserSession userSession, Func<int, bool> canViewProduct)
        {
            CustId = userSession.CustId;
            UserLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            Currency = userSession.Currency;
            CanViewReports = userSession.SubPageAccess.ViewReports;
            CanViewProduct = canViewProduct;
        }

        public int CustId { get; }

        public UserLevel UserLevel { get; }

        public string Currency { get; }

        public bool CanViewReports { get; }

        public Func<int, bool> CanViewProduct { get; }
    }
}