using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus
{
    public class CustomerNonProductStatusCommand : BaseQuery, IRequest<Result>
    {
        public int SearchAccountId { get; set; }

        public byte SearchAccountRoleId { get; set; }

        public bool StatusValue { get; set; }

        public string StatusName { get; set; }

        public int ServerId { get; }

        public override bool IsValid()
        {
            var isValid = base.IsValid() && !string.IsNullOrWhiteSpace(StatusName) && CanUpdateAccount && !IsInternal && SearchAccountId > 0;
            return !StatusName.Equals(StatusNames.DoubleComm) ? isValid : isValid && IsValidDoubleComm;
        }

        internal bool IsValidDoubleComm => userSession.RoleId == SearchAccountRoleId + 1;
    }
}