using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Features.Password
{
    public class ResetMainAccountPasswordQuery : IRequest<PasswordViewModel>
    {
        public int CustId { get; set; }

        public byte CustRoleId { get; set; }

        public string Username { get; set; }

        public bool IsValid() => UserLevel.Member.Value <= CustRoleId && CustRoleId <= UserLevel.Super.Value;
    }
}