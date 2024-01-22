using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public class UpdateSubAccountPageAccessRequest : SubAccountBaseRequest, IRequest<Result>
    {
        public string PageAccess { get; set; }
    }
}