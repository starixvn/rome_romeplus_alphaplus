using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public class RemoveSubAccountOtpRequest : SubAccountBaseRequest, IRequest<Result>
    {
    }
}