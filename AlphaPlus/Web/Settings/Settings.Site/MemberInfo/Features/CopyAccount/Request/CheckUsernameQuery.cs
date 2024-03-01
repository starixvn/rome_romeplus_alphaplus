using AlphaPlus.Settings.Site.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.Request
{
    public class CheckUsernameQuery : MemberInfoBaseQuery, IRequest<Result>
    {
        public CheckUsernameQuery(IUser viewer) 
            : base(viewer)
        {
        }
    }
}