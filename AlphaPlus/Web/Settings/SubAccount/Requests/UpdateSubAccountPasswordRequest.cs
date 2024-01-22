using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public class UpdateSubAccountPasswordRequest : SubAccountBaseRequest, IRequest<Result>
    {
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}