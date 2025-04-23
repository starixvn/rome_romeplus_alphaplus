using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.SubAccount.Requests
{
    public class AddSubAccountRequest : SubAccountBaseRequest, IRequest<Result>
    {
        public string First { get; set; }

        public string Next { get; set; }

        public string Username => $"{userSession.UserName}Sub{First}{Next}";

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string PageAccess { get; set; }

        public int RoleId => userSession.RoleId;

        public int CustDBGroupId => userSession.CustDBGroupId;

        public override bool IsValid()
        {
            return base.IsValid() && HasUpdatePermission;
        }
    }
}