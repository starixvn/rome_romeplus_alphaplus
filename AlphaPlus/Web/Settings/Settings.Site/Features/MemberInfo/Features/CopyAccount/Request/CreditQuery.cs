using AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount.Request
{
    public class CreditQuery : MemberInfoBaseQuery, IRequest<CreditInfo>
    {
        public CreditQuery() : this(null){}
        
        public CreditQuery(IUser viewer) 
            : base(viewer)
        {
        }
    }
}