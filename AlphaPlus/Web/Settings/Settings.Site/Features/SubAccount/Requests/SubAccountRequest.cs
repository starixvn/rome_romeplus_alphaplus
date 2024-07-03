using AlphaPlus.Settings.Site.Features.SubAccount.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.SubAccount.Requests
{
    public class SubAccountRequest : SubAccountBaseRequest, IRequest<SubAccountViewModel>
    {
    }
}