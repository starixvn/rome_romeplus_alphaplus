using Alpha.MemberInfo.Site.Features.CopyAccount.Models;
using Alpha.MemberInfo.Site.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace Alpha.MemberInfo.Site.Features.CopyAccount.Request
{
    public class CreditQuery : MemberInfoBaseQuery, IRequest<CreditInfo>
    {
        public CreditQuery(IUser viewer) 
            : base(viewer)
        {
        }
    }
}