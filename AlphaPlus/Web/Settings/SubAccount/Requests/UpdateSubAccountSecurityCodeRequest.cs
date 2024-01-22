using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public class UpdateSubAccountSecurityCodeRequest : SubAccountBaseRequest, IRequest<Result>
    {
        public string NewSecurityCode { get; set; }

        public string ConfirmSecurityCode { get; set; }
    }
}