namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.IndirectLoginAccount
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ValidateAccountQuery : MemberInfoBaseQuery, IRequest<Result>
    {
        public ValidateAccountQuery() : this(null){}
        
        public ValidateAccountQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
        {
            return base.IsValid() && !IsInternal && IsAllowedIndirectLogin;
        }
    }
}