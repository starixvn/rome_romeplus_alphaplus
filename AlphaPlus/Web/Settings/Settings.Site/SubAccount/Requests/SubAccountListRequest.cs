using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.SubAccount.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public class SubAccountListRequest : IRequest<IEnumerable<SubAccountViewModel>>
    {
        public int CustId { get; set; } = UserSession.Current.CustId;

        public bool IsValid() => CustId > 0;
    }
}