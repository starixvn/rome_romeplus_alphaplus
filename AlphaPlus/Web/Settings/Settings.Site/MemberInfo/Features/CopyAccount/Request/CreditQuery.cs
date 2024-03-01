using AlphaPlus.Settings.Site.Features.CopyAccount.Models;
using AlphaPlus.Settings.Site.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.Request
{
    public class CreditQuery : MemberInfoBaseQuery, IRequest<CreditInfo>
    {
        public CreditQuery(IUser viewer) 
            : base(viewer)
        {
        }
    }
}