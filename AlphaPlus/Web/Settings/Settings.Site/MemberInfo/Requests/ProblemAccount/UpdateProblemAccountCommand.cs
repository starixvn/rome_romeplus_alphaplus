namespace Alpha.MemberInfo.Site.Requests.ProblemAccount
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class UpdateProblemAccountCommand : MemberInfoBaseQuery, IRequest<Result>
    {
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