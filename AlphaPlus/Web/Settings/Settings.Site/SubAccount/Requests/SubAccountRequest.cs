using AlphaPlus.Settings.Site.SubAccount.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public class SubAccountRequest : SubAccountBaseRequest, IRequest<SubAccountViewModel>
    {
        public int CustId { get; set; }
    }
}