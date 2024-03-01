namespace Alpha.MemberInfo.Site.Requests.IndirectLoginAccount
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ValidateAccountQuery : MemberInfoBaseQuery, IRequest<Result>
    {
        public ValidateAccountQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
        {
            return base.IsValid() && !IsInternal && IsAllowedIndirectLogin;
        }
    }
}