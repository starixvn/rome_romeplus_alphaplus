using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount.Request
{
    public class CheckUsernameQuery : MemberInfoBaseQuery, IRequest<Result>
    {
        public CheckUsernameQuery() : this(null){}

        public CheckUsernameQuery(IUser viewer) 
            : base(viewer)
        {
        }
    }
}