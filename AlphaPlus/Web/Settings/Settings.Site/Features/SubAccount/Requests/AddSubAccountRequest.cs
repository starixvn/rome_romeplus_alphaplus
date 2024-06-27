using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.SubAccount.Requests
{
    public class AddSubAccountRequest : SubAccountBaseRequest, IRequest<Result>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string PageAccess { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && HasUpdatePermission;
        }
    }
}