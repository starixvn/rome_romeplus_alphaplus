using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount
{
    public class UpdateProblemAccountCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public UpdateProblemAccountCommand() : this(null) { }

        public UpdateProblemAccountCommand(IUser viewer) : base(viewer)
        {
        }

        public bool IsAffectedAllDownlines { get; set; }

        public string Ids { get; set; }

        public byte RoleId { get; set; }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}