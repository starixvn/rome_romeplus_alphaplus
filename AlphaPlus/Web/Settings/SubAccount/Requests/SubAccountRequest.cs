using AlphaPlus.Settings.Site.SubAccount.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public class SubAccountRequest : IRequest<IEnumerable<SubAccountViewModel>>
    {
        public int CustId { get; set; }

        public string PageAccess { get; set; }
    }
}