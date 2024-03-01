using Alpha.MemberInfo.Site.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace Alpha.MemberInfo.Site.Features.CopyAccount.Request
{
    public class CheckExistedCustomerQuery : MemberInfoBaseQuery, IRequest<bool>
    {
        public CheckExistedCustomerQuery(IUser viewer) 
            : base(viewer)
        {
        }
    }
}