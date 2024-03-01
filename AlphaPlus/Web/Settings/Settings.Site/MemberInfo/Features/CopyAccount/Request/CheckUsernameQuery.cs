using Alpha.MemberInfo.Site.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace Alpha.MemberInfo.Site.Features.CopyAccount.Request
{
    public class CheckUsernameQuery : MemberInfoBaseQuery, IRequest<Result>
    {
        public CheckUsernameQuery(IUser viewer) 
            : base(viewer)
        {
        }
    }
}